﻿//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// MediaFileController.cs
//

using ASMR.Core.Constants;
using ASMR.Core.Generic;
using ASMR.Web.Controllers.Generic;
using ASMR.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Controllers.API;

public class MediaFileController : DefaultAbstractApiController<MediaFileController>
{
	private readonly IMediaFileService _mediaFileService;

	public MediaFileController(ILogger<MediaFileController> logger, IMediaFileService mediaFileService) : base(logger)
	{
		_mediaFileService = mediaFileService;
	}

	[HttpGet("{id}"), ResponseCache(CacheProfileName = "MediaFileCache")]
	public async Task<IActionResult> Download(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.RequiredParameterNotProvided,
				"Please provide media file identifier.");
			return BadRequest(new DefaultResponseModel(errorModel));
		}

		var mediaFile = await _mediaFileService.GetMediaFileById(id);
		if (mediaFile is null)
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The media file you are looking for is not found.");
			return BadRequest(new DefaultResponseModel(errorModel));
		}

		var filePath = Path.Combine(Directory.GetCurrentDirectory(), mediaFile.Location);
		if (!System.IO.File.Exists(filePath))
		{
			var errorModel = new ResponseError(ErrorCodeConstants.ResourceNotFound,
				"The resource file you are looking for is not found.");
			return BadRequest(new DefaultResponseModel(errorModel));
		}

		var lastModified = mediaFile.LastUpdatedAt ?? mediaFile.CreatedAt;
		Response.GetTypedHeaders().LastModified = lastModified;

		var requestHeaders = Request.GetTypedHeaders();
		if (requestHeaders.IfModifiedSince.HasValue && requestHeaders.IfModifiedSince.Value >= lastModified)
		{
			return StatusCode(StatusCodes.Status304NotModified);
		}

		var file = System.IO.File.OpenRead(filePath);

		return File(file, mediaFile.MimeType, mediaFile.Name);
	}
}