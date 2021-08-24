using System;
using System.IO;
using System.Threading.Tasks;
using ASMR.Core.Constants;
using ASMR.Core.Generic;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.ReleaseInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// ReSharper disable RouteTemplates.ActionRoutePrefixCanBeExtractedToControllerRoute
namespace ASMR.Web.Controllers.API
{
	public class ReleaseController: DefaultAbstractApiController<ReleaseController>
	{
		public ReleaseController(ILogger<ReleaseController> logger) : base(logger)
		{
		}

		[HttpGet("mobile")]
		public async Task<IActionResult> GetMobileReleaseInformation()
		{
			var releaseInformation = await ReleaseInformationManager.GetASMRMobileReleaseInformation();
			// ReSharper disable once InvertIf
			if (releaseInformation is null)
			{
				return Ok(new DefaultResponseModel
				{
					Message = "Release information for mobile applications currently is not available."
				});
			}

			return Ok(new DefaultResponseModel<ASMRMobileReleaseInformation>(releaseInformation));
		}
		
		[HttpGet("mobile/update")]
		public async Task<IActionResult> GetMobileReleaseUpdate([FromQuery] string platform,
			[FromQuery] string currentVersion,
			[FromQuery] bool download = true)
		{
			var useStructuredModel = !string.IsNullOrEmpty(currentVersion);

			if (string.IsNullOrEmpty(platform))
			{
				var message = "Required parameter 'platform' is not satisfied.";
				var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided, message);
				return BadRequest(useStructuredModel ? new DefaultResponseModel(errorModel) : message);
			}

			if (platform.ToUpper() != "ANDROID" && platform.ToUpper() != "IOS")
			{
				var message = $"Platform '{platform}' is not valid. Only 'Android' and 'iOS' platform is supported.";
				var errorModel = new ResponseError(ErrorCodeConstants.ModelValidationFailed, message);
				return BadRequest(useStructuredModel ? new DefaultResponseModel(errorModel) : message);
			}
			
			var releaseInformation = await ReleaseInformationManager.GetASMRMobileReleaseInformation();
			var hasReleaseInformation = releaseInformation is not null;
			var releasedVersion = platform.ToUpper() switch
			{
				"ANDROID" => hasReleaseInformation ? releaseInformation.Android.Version : null,
				"IOS" => hasReleaseInformation ? releaseInformation.iOS.Version : null,
				_ => null
			};

			var fromVersion = !string.IsNullOrEmpty(currentVersion) ? new Version(currentVersion) : null;
			var toVersion = !string.IsNullOrEmpty(releasedVersion) ? new Version(releasedVersion) : null;
			
			if (!hasReleaseInformation || toVersion is null)
			{
				const string message = "Release information for mobile applications currently is not available.";
				return Ok(useStructuredModel ? new DefaultResponseModel
				{
					Message = message
				} : message);
			}
			
			var directDownloadAvailable = platform.ToUpper() switch
			{
				"ANDROID" => releaseInformation.Android.DirectDownload.Available,
				"IOS" => releaseInformation.Android.DirectDownload.Available,
				_ => false
			};
			var platformName = platform.ToUpper() switch
			{
				"ANDROID" => "Android",
				"IOS" => "iOS",
				_ => "your platform"
			};
			var platformExtension = platform.ToUpper() switch
			{
				"ANDROID" => ".apk",
				"IOS" => ".ipa",
				_ => ""
			};
			var platformMimeType =  platform.ToUpper() switch
			{
				"ANDROID" => "application/vnd.android.package-archive",
				"IOS" => "application/octet-stream",
				_ => ""
			};
			if (fromVersion is not null && fromVersion >= toVersion)
			{
				var message = $"You already have the latest mobile application for {platformName}.";
				return Ok(useStructuredModel ? new DefaultResponseModel
				{
					Message = message
				} : message);
			}

			var applicationFileName = $"ASMR.Mobile.{platformName}-{releasedVersion}{platformExtension}";
			var applicationFilePath = Path.Join(Directory.GetCurrentDirectory(),
				"ReleaseInformation", "DirectStore", applicationFileName);
			if (!directDownloadAvailable || !System.IO.File.Exists(applicationFilePath))
			{
				var message = $"ASMR for {platform} (version {releasedVersion}) is not available at the moment.";
				return Ok(useStructuredModel ? new DefaultResponseModel
				{
					Message = message
				} : message);
			}

			if (!download)
			{
				var message = $"ASMR for {platform} (version {releasedVersion}) is available for download.";
				return Ok(useStructuredModel ? new DefaultResponseModel
				{
					Message = message
				} : message);
			}

			var applicationFile = System.IO.File.OpenRead(applicationFilePath);
			return File(applicationFile, platformMimeType, applicationFileName);
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
}