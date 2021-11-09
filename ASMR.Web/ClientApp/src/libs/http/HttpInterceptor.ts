import HttpResponse from "@asmr/libs/http/HttpResponse"
import HttpRequestInterceptor from "@asmr/libs/http/HttpRequestInterceptor"
import HttpResponseInterceptor from "@asmr/libs/http/HttpResponseInterceptor"
import HttpErrorInterceptor from "@asmr/libs/http/HttpErrorInterceptor"

type HttpInterceptor = {
	request: HttpRequestInterceptor
	response: HttpResponseInterceptor
	error: HttpErrorInterceptor
}

export const DefaultHttpInterceptor: HttpInterceptor = {
	error: (error) => error,
	request: (options, body) => [options, body],
	response: <TResponse>(response: HttpResponse<TResponse>) => response,
}

export default HttpInterceptor
