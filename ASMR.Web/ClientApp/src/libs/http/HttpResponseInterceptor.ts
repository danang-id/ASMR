import HttpResponse from "@asmr/libs/http/HttpResponse"

type HttpResponseInterceptor = <TResponse = unknown>(response: HttpResponse<TResponse>)
	=> HttpResponse<TResponse> | Promise<HttpResponse<TResponse>>

export default HttpResponseInterceptor
