using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ASMR.Web.Configurations;
using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASMR.Web.Services;

internal class CaptchaVerificationResult
{
	[JsonPropertyName("success")] public bool Success { get; set; }

	[JsonPropertyName("challenge_ts")] public DateTimeOffset ChallengeTimestamp { get; set; }

	[JsonPropertyName("hostname")] public string HostName { get; set; }

	[JsonPropertyName("error-codes")] public IEnumerable<string> ErrorCodes { get; set; }
}

public interface ICaptchaService
{
	public Task<bool> VerifyCaptcha(string captchaResponseToken);
}

public class CaptchaService : ICaptchaService
{
	private const string BaseUrl = "https://www.google.com/";
	private static readonly HttpClient HttpClient = new();

	private readonly ILogger<CaptchaService> _logger;
	private readonly CaptchaOptions _options;

	public CaptchaService(ILogger<CaptchaService> logger, IOptions<CaptchaOptions> options)
	{
		_logger = logger;
		_options = options.Value;
	}

	public async Task<bool> VerifyCaptcha(string captchaResponseToken)
	{
		try
		{
			var url = new Url(BaseUrl)
				.AppendPathSegments("recaptcha", "api", "siteverify")
				.SetQueryParams(new
				{
					secret = _options.SecretKey,
					response = captchaResponseToken
				});
			var response = await HttpClient.PatchAsync(url, null!);
			var responseContent = await response.Content.ReadAsByteArrayAsync();

			var verificationResult = JsonSerializer.Deserialize<CaptchaVerificationResult>(responseContent);
			if (verificationResult is null)
			{
				return false;
			}

			if (verificationResult.ErrorCodes is not null &&
			    verificationResult.ErrorCodes.Any())
			{
				foreach (var errorCode in verificationResult.ErrorCodes)
				{
					_logger.LogError("CAPTCHA verification failed: {ErrorCode}", errorCode);
				}
			}

			return verificationResult.Success;
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Failed to validate captcha");
			return false;
		}
	}
}