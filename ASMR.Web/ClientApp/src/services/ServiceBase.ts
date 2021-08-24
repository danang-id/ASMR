import Cookie from "js-cookie"
import HttpClient from "@asmr/libs/http/HttpClient"
import HttpResponse from "@asmr/libs/http/HttpResponse";
import Platform from "@asmr/data/enumerations/Platform"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import { SetProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import HttpRequestBody from "@asmr/libs/http/HttpRequestBody"
import HttpRequestOptions from "@asmr/libs/http/HttpRequestOptions"
import config from "@asmr/libs/common/config"

class ServiceBase {
	private readonly crsfRequestTokenCookieName = "ASMR.CSRF-Request-Token"
	private readonly csrfTokenFieldName = "__CSRF-Token__"
	private readonly csrfTokenHeaderName = "X-CSRF-Token"

	protected csrfToken?: string
	protected readonly httpClient: HttpClient
	protected readonly setProgress: SetProgressInfo

	constructor(controller?: AbortController, setProgress?: SetProgressInfo) {
		this.saveCsrfRequestToken()
		this.httpClient = new HttpClient({
			baseUrl: config.backEndBaseUrl ?? window.location.origin,
			mode: config.backEndBaseUrl ? "cors" : "same-origin",
			signal: controller ? controller.signal : void 0,
		})
		this.setProgress = setProgress ?? (() => {})

		const getFunction = this.httpClient.get.bind(this.httpClient)
		const postFunction = this.httpClient.post.bind(this.httpClient)
		const putFunction = this.httpClient.put.bind(this.httpClient)
		const patchFunction = this.httpClient.patch.bind(this.httpClient)
		const deleteFunction = this.httpClient.delete.bind(this.httpClient)
		this.httpClient.get = (endpoint, options) => {
			const newHeader = this.appendRequestToken(undefined, options?.headers)[1]
			const newParams = this.appendClientInformation(options?.params)
			const newOptions: HttpRequestOptions = !options ? {
				headers: newHeader,
				params: newParams
			} : {
				...options,
				headers: newHeader,
				params: newParams
			}
			return getFunction(endpoint, newOptions)
		}
		this.httpClient.post = (endpoint, body, options) => {
			const [newBody, newHeader] = this.appendRequestToken(body, options?.headers)
			const newParams = this.appendClientInformation(options?.params)
			const newOptions: HttpRequestOptions = !options ? {
				headers: newHeader,
				params: newParams
			} : {
				...options,
				headers: newHeader,
				params: newParams
			}
			return postFunction(endpoint, newBody, newOptions)
		}
		this.httpClient.put = (endpoint, body, options) => {
			const [newBody, newHeader] = this.appendRequestToken(body, options?.headers)
			const newParams = this.appendClientInformation(options?.params)
			const newOptions: HttpRequestOptions = !options ? {
				headers: newHeader,
				params: newParams
			} : {
				...options,
				headers: newHeader,
				params: newParams
			}
			return putFunction(endpoint, newBody, newOptions)
		}
		this.httpClient.patch = (endpoint, body, options) => {
			const [newBody, newHeader] = this.appendRequestToken(body, options?.headers)
			const newParams = this.appendClientInformation(options?.params)
			const newOptions: HttpRequestOptions = !options ? {
				headers: newHeader,
				params: newParams
			} : {
				...options,
				headers: newHeader,
				params: newParams
			}
			return patchFunction(endpoint, newBody, newOptions)
		}
		this.httpClient.delete = (endpoint, options) => {
			const newHeader = this.appendRequestToken(undefined, options?.headers)[1]
			const newParams = this.appendClientInformation(options?.params)
			const newOptions: HttpRequestOptions = !options ? {
				headers: newHeader,
				params: newParams
			} : {
				...options,
				headers: newHeader,
				params: newParams
			}
			return deleteFunction(endpoint, newOptions)
		}
	}

	private saveCsrfRequestToken() {
		this.csrfToken = Cookie.get(this.crsfRequestTokenCookieName)
	}

	protected appendRequestToken(body?: HttpRequestBody, headers: HeadersInit = {}): [HttpRequestBody | undefined, HeadersInit] {
		if (this.csrfToken) {
			if (body && body instanceof  FormData) {
				body.append(this.csrfTokenFieldName, this.csrfToken)
			}
			if (headers) {
				(headers as Record<string, string>)[this.csrfTokenHeaderName] = this.csrfToken
			}
		}

		return [body, headers]
	}

	protected appendClientInformation(params?: URLSearchParams | Record<string, any>): URLSearchParams {
		params = new URLSearchParams(params)
		params.append("clientPlatform", Platform.Web)
		params.append("clientVersion", config.application.version)
		return params as URLSearchParams
	}

	protected createFormData(body?: Record<string, any>) {
		const formData = new FormData()
		if (body) {
			for (const bodyKey in body) {
				// eslint-disable-next-line no-prototype-builtins
				if (body.hasOwnProperty(bodyKey)) {
					const bodyValue = body[bodyKey]
					if (Array.isArray(bodyValue)) {
						for (const value of bodyValue) {
							formData.append(bodyKey, value)
						}
					} else {
						formData.append(bodyKey, bodyValue)
					}
				}
			}
		}
		return formData
	}

	protected async request<TResponse extends DefaultResponseModel>(
		requester: () => Promise<HttpResponse<TResponse>>
	): Promise<TResponse> {
		try {
			this.setProgress(true, 0)
			const response = await requester()
			this.saveCsrfRequestToken()
			this.setProgress(true, 1)
			return response.body as TResponse
		} finally {
			this.setProgress(false, 0)
		}
	}
}

export default ServiceBase
