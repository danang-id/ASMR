import { EventEmitter } from "events"

import HttpClientOptions, { DefaultHttpClientOptions } from "@asmr/libs/http/HttpClientOptions"
import HttpInterceptor, { DefaultHttpInterceptor } from "@asmr/libs/http/HttpInterceptor"
import HttpMethod from "@asmr/libs/http/HttpMethod"
import HttpRequestBody from "@asmr/libs/http/HttpRequestBody"
import HttpRequestOptions from "@asmr/libs/http/HttpRequestOptions"
import HttpResponse from "@asmr/libs/http/HttpResponse"
import HttpErrorInterceptor from "@asmr/libs/http/HttpErrorInterceptor"
import HttpResponseInterceptor from "@asmr/libs/http/HttpResponseInterceptor"
import HttpRequestInterceptor from "@asmr/libs/http/HttpRequestInterceptor"

class HttpClient extends EventEmitter {
	private readonly options: HttpClientOptions
	private readonly interceptor: HttpInterceptor

	constructor(options: HttpClientOptions = DefaultHttpClientOptions) {
		super()
		this.options = { ...DefaultHttpClientOptions, ...options }
		this.interceptor = DefaultHttpInterceptor
	}

	private static createRequestInit(
		method: HttpMethod,
		options: HttpRequestOptions = {},
		body?: HttpRequestBody
	): RequestInit {
		const requestInit: RequestInit = {
			...options,
			method: method
		}
		if (typeof body === "undefined" || body === null) {
			return requestInit
		}

		if (body instanceof FormData) {
			requestInit.body = body
			requestInit.headers = {
				// "Content-Type": "multipart/form-data",
				...requestInit.headers
			}
		} else if (body instanceof URLSearchParams) {
			requestInit.body = body
			requestInit.headers = {
				"Content-Type": "application/x-www-form-urlencoded",
				...requestInit.headers
			}
		} else if (typeof body === "object") {
			requestInit.body = JSON.stringify(body)
			requestInit.headers = {
				"Content-Type": "application/json",
				...requestInit.headers
			}
		} else {
			requestInit.body = body
			requestInit.headers = {
				"Content-Type": "text/plain",
				...requestInit.headers
			}
		}

		return requestInit
	}

	private static createResponseBody<TResponse = unknown>(response: Response)
		: Promise<TResponse | FormData | string | undefined> {
		if (response.bodyUsed) {
			return Promise.resolve(void 0)
		}

		const clonedResponse = response.clone()
		const contentType = clonedResponse.headers.get("Content-Type")
		if (!contentType) {
			return clonedResponse.text()
		}

		if (contentType.startsWith("application/json")) {
			return clonedResponse.json() as Promise<TResponse>
		}

		if (contentType.startsWith("multipart/form-data")) {
			return clonedResponse.formData()
		}

		return clonedResponse.text()
	}

	private getURL(endpoint: string, options: HttpRequestOptions): URL {
		const url = new URL(endpoint, this.options.baseUrl)
		const urlSearchParams = new URLSearchParams(options.params ?? {})
		urlSearchParams.forEach((value, name) => {
			url.searchParams.append(name, value)
		})
		return url
	}

	private getCombinedRequestOptions(options?: HttpRequestOptions): HttpRequestOptions {
		return { ...this.options, ...options }
	}

	private async interceptError(error: Error) {
		const newError = await this.interceptor.error(error)
		this.emit("error", newError)
		return newError
	}

	private async interceptRequest(options?: HttpRequestOptions, body?: HttpRequestBody) {
		const newRequest = await this.interceptor.request(this.getCombinedRequestOptions(options), body)
		this.emit("request", ...newRequest)
		return newRequest
	}

	private async interceptResponse<TResponse = unknown>(response: HttpResponse<TResponse>) {
		const newResponse = await this.interceptor.response(response)
		this.emit("response", newResponse)
		return newResponse
	}

	public setErrorInterceptor(interceptor: HttpErrorInterceptor): HttpClient {
		this.interceptor.error = interceptor
		return this
	}

	public setRequestInterceptor(interceptor: HttpRequestInterceptor): HttpClient {
		this.interceptor.request = interceptor
		return this
	}

	public setResponseInterceptor(interceptor: HttpResponseInterceptor): HttpClient {
		this.interceptor.response = interceptor
		return this
	}

	public async request<TResponse = unknown>(
		method: HttpMethod,
		endpoint: string,
		body?: HttpRequestBody,
		options?: HttpRequestOptions
	): Promise<HttpResponse<TResponse>> {
		try {
			const [_options, _body] = await this.interceptRequest(options, body)

			const url = this.getURL(endpoint, _options)
			const requestInit = HttpClient.createRequestInit(method, _options, _body)
			const response = await fetch(url.toString(), requestInit)
			const responseBody = await HttpClient.createResponseBody<TResponse>(response)

			return await this.interceptResponse(new HttpResponse<TResponse>(response, responseBody))
		} catch (error) {
			throw await this.interceptError(error as Error)
		}
	}

	public get<TResponse = unknown>(
		endpoint: string,
		options?: HttpRequestOptions
	): Promise<HttpResponse<TResponse>> {
		return this.request(HttpMethod.GET, endpoint, void 0, options)
	}

	public post<TResponse = unknown>(
		endpoint: string,
		body?: HttpRequestBody,
		options?: HttpRequestOptions
	): Promise<HttpResponse<TResponse>> {
		return this.request(HttpMethod.POST, endpoint, body, options)
	}

	public put<TResponse = unknown>(
		endpoint: string,
		body?: HttpRequestBody,
		options?: HttpRequestOptions
	): Promise<HttpResponse<TResponse>> {
		return this.request(HttpMethod.PUT, endpoint, body, options)
	}

	public patch<TResponse = unknown>(
		endpoint: string,
		body?: HttpRequestBody,
		options?: HttpRequestOptions
	): Promise<HttpResponse<TResponse>> {
		return this.request(HttpMethod.PATCH, endpoint, body, options)
	}

	public delete<TResponse = unknown>(
		endpoint: string,
		options?: HttpRequestOptions
	): Promise<HttpResponse<TResponse>> {
		return this.request(HttpMethod.DELETE, endpoint, void 0, options)
	}
}

export default HttpClient
