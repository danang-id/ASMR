using System;
using System.Diagnostics;
using System.Text.Json;
using ASMR.Common.Constants;
using ASMR.Core.Constants;
using ASMR.Core.Generic;

namespace ASMR.Mobile.Extensions
{
    public static class ExceptionExtension
    {
        public static TResponse ToResponseModel<TResponse>(this Exception exception)
            where TResponse : class
        {
            Debug.WriteLine(exception.Message, typeof(Exception).Name);
            var error = new ResponseError(ErrorCodeConstants.GenericClientError, exception.Message);
            var jsonError = JsonSerializer.Serialize(new DefaultResponseModel(error),
                    JsonConstants.DefaultJsonSerializerOptions);
            return JsonSerializer.Deserialize<TResponse>(jsonError,
                JsonConstants.DefaultJsonSerializerOptions);
        }
    }
}
