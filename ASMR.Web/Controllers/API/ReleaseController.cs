using System;
using System.IO;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Enumerations;
using ASMR.Core.Generic;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Extensions;
using ASMR.Web.ReleaseInformation;
using ASMR.Web.ReleaseInformation.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
namespace ASMR.Web.Controllers.API;

public class ReleaseController : DefaultAbstractApiController<ReleaseController>
{
	public ReleaseController(ILogger<ReleaseController> logger) : base(logger)
	{
	}

	[HttpGet("mobile/update")]
	public async Task<IActionResult> GetMobileReleaseUpdate([FromQuery] string currentVersion,
		[FromQuery] bool download = true)
	{
		var useStructuredModel = !string.IsNullOrEmpty(currentVersion);
		var clientPlatform = Request.GetClientPlatform();

		if (clientPlatform is (ClientPlatform)(-1) or ClientPlatform.Web)
		{
			var message = $"Platform '{clientPlatform}' is not valid. Only 'Android' and 'iOS' platform is supported.";
			var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed, message);
			return BadRequest(useStructuredModel ? new DefaultResponseModel(errorModel) : message);
		}

		var releaseInformation = await ReleaseInformationManager.GetASMRMobileReleaseInformation();
		var hasReleaseInformation = releaseInformation is not null;
		var releasedVersion = clientPlatform switch
		{
			ClientPlatform.Android => hasReleaseInformation ? releaseInformation.Android.Version : null,
			ClientPlatform.iOS => hasReleaseInformation ? releaseInformation.iOS.Version : null,
			_ => null
		};
		var releasedVersionCode = clientPlatform switch
		{
			ClientPlatform.Android => hasReleaseInformation ? releaseInformation.Android.VersionCode : 0,
			ClientPlatform.iOS => hasReleaseInformation ? releaseInformation.iOS.VersionCode : 0,
			_ => 0
		};

		var fromVersion = !string.IsNullOrEmpty(currentVersion) ? new Version(currentVersion) : null;
		var toVersion = !string.IsNullOrEmpty(releasedVersion) ? new Version(releasedVersion) : null;

		if (!hasReleaseInformation || toVersion is null)
		{
			const string message = "Release information for mobile applications currently is not available.";
			return Ok(useStructuredModel
				? new DefaultResponseModel
				{
					Message = message
				}
				: message);
		}

		var directDownloadAvailable = clientPlatform switch
		{
			ClientPlatform.Android => releaseInformation.Android.DirectDownload.Available,
			ClientPlatform.iOS => releaseInformation.Android.DirectDownload.Available,
			_ => false
		};
		var platformName = clientPlatform.ToString();
		var platformExtension = clientPlatform switch
		{
			ClientPlatform.Android => ".apk",
			ClientPlatform.iOS => ".ipa",
			_ => ""
		};
		var platformMimeType = clientPlatform switch
		{
			ClientPlatform.Android => "application/vnd.android.package-archive",
			ClientPlatform.iOS => "application/octet-stream",
			_ => ""
		};
		if (fromVersion is not null && fromVersion >= toVersion)
		{
			var message = $"You already have the latest mobile application for {platformName}.";
			return Ok(useStructuredModel
				? new DefaultResponseModel
				{
					Message = message
				}
				: message);
		}

		var applicationFileName =
			$"asmr_mobile_{platformName.ToLower()}_v{releasedVersion}-{releasedVersionCode}{platformExtension}";
		var applicationFilePath = Path.Join(Directory.GetCurrentDirectory(),
			"ReleaseInformation", "DirectStore", applicationFileName);
		if (!directDownloadAvailable || !System.IO.File.Exists(applicationFilePath))
		{
			var message =
				$"asmr for {clientPlatform} (v{releasedVersion}-{releasedVersionCode}) is not available at the moment.";
			return Ok(useStructuredModel
				? new DefaultResponseModel
				{
					Message = message
				}
				: message);
		}

		if (!download)
		{
			var message =
				$"asmr for {clientPlatform} (v{releasedVersion}-{releasedVersionCode}) is available for download.";
			return Ok(useStructuredModel
				? new DefaultResponseModel
				{
					Message = message
				}
				: message);
		}

		var applicationFile = System.IO.File.OpenRead(applicationFilePath);
		return File(applicationFile, platformMimeType, applicationFileName);
	}

	[HttpGet("mobile")]
	public async Task<IActionResult> GetMobileReleaseInformation()
	{
		var clientPlatform = Request.GetClientPlatform();

		var releaseInformation = await ReleaseInformationManager.GetASMRMobileReleaseInformation();
		// ReSharper disable once InvertIf
		if (releaseInformation is null)
		{
			return Ok(new DefaultResponseModel
			{
				Message = "Release information for mobile applications currently is not available."
			});
		}

		return clientPlatform switch
		{
			ClientPlatform.Android => Ok(
				new DefaultResponseModel<AndroidReleaseInformation>(releaseInformation.Android)),
			ClientPlatform.iOS => Ok(new DefaultResponseModel<iOSReleaseInformation>(releaseInformation.iOS)),
			_ => Ok(new DefaultResponseModel<ASMRMobileReleaseInformation>(releaseInformation))
		};
	}

	[HttpGet("web")]
	public async Task<IActionResult> GetWebReleaseInformation()
	{
		var releaseInformation = await ReleaseInformationManager.GetASMRWebReleaseInformation();
		// ReSharper disable once InvertIf
		if (releaseInformation is null)
		{
			return Ok(new DefaultResponseModel
			{
				Message = "Release information for web applications currently is not available."
			});
		}

		return Ok(new DefaultResponseModel<ASMRWebReleaseInformation>(releaseInformation));
	}
}