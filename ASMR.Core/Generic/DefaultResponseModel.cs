//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// DefaultResponseModel.cs
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using ASMR.Common.Constants;
using ASMR.Core.Constants;

namespace ASMR.Core.Generic;

public class DefaultResponseModel<T> : IResponseModel<T>, IErrorResponseModel
	where T : class
{
	public DefaultResponseModel()
	{
		IsSuccess = true;
		Message = null;
		Data = null;
		Errors = null;
	}

	public DefaultResponseModel(T data)
	{
		IsSuccess = true;
		Message = null;
		Data = data;
		Errors = null;
	}

	public DefaultResponseModel(ResponseError error)
	{
		IsSuccess = false;
		Message = null;
		Data = null;
		Errors = new Collection<ResponseError> { error };
	}

	public DefaultResponseModel(IEnumerable<ResponseError> errors)
	{
		IsSuccess = false;
		Message = null;
		Data = null;
		Errors = errors;
	}

	public DefaultResponseModel(Exception exception)
	{
		IsSuccess = false;
		Message = null;
		Data = null;
		Errors = new Collection<ResponseError>
		{
			new ResponseError(ErrorCodeConstants.GenericServerError, exception.Message)
		};
	}

	public DefaultResponseModel(IEnumerable<Exception> exceptions)
	{
		IsSuccess = false;
		Message = null;
		Data = null;
		Errors = exceptions
			.Select(exception => new ResponseError(ErrorCodeConstants.GenericServerError, exception.Message));
	}


	public bool IsSuccess { get; set; }

	public string Message { get; set; }

	public T Data { get; set; }

	public IEnumerable<ResponseError> Errors { get; set; }

	public override string ToString()
	{
		return JsonSerializer.Serialize(this, JsonConstants.DefaultJsonSerializerOptions);
	}

	public string ToJsonString()
	{
		return JsonSerializer.Serialize(this, JsonConstants.IndentedJsonSerializerOptions);
	}
}

public class DefaultResponseModel : DefaultResponseModel<object>
{
	public DefaultResponseModel()
	{
	}

	public DefaultResponseModel(object data) : base(data)
	{
	}

	public DefaultResponseModel(ResponseError error) : base(error)
	{
	}

	public DefaultResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}

	public DefaultResponseModel(Exception exception) : base(exception)
	{
	}
}