import HttpRequestOptions from "@asmr/libs/http/HttpRequestOptions"
import HttpRequestBody from "@asmr/libs/http/HttpRequestBody"

type HttpRequestInterceptorResult = [HttpRequestOptions, HttpRequestBody | undefined]

type HttpRequestInterceptor = (options: HttpRequestOptions, body?: HttpRequestBody)
	=> HttpRequestInterceptorResult | Promise<HttpRequestInterceptorResult>

export default HttpRequestInterceptor
