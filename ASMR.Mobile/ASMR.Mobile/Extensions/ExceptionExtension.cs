using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
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
            var error = new ResponseError(ErrorCodeConstants.GenericClientError, exception.Message);
            var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new DefaultResponseModel(error),
                    JsonConstants.DefaultJsonSerializerOptions);
            return JsonSerializer.Deserialize<DefaultResponseModel>(jsonUtf8Bytes,
                JsonConstants.DefaultJsonSerializerOptions) as TResponse;
        }

        public static async Task<TResponse> ToResponseModelAsync<TResponse>(this Exception exception)
            where TResponse : class
        {
            await using var stream = new MemoryStream();

            var error = new ResponseError(ErrorCodeConstants.GenericClientError, exception.Message);
            await JsonSerializer.SerializeAsync(stream, new DefaultResponseModel(error),
                JsonConstants.DefaultJsonSerializerOptions);
            return await JsonSerializer.DeserializeAsync<DefaultResponseModel>(stream,
                JsonConstants.DefaultJsonSerializerOptions) as TResponse;
        }
    }
}
