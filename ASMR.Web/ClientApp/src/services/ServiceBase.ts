import Cookie from "js-cookie"
import HttpClient from "@asmr/libs/http/HttpClient"
import HttpResponse from "@asmr/libs/http/HttpResponse";
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import { SetProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import HttpRequestBody from "@asmr/libs/http/HttpRequestBody"

class ServiceBase {
	protected readonly csrfToken?: string
	protected readonly httpClient: HttpClient
	protected readonly setProgress: SetProgressInfo

	constructor(controller?: AbortController, setProgress?: SetProgressInfo) {
		this.csrfToken = Cookie.get("ASMR.CSRF-Request-Token")
		this.httpClient = new HttpClient({
			signal: controller ? controller.signal : void 0
		})
		this.setProgress = setProgress ?? (() => {})

		const postFunction = this.httpClient.post.bind(this.httpClient)
		const putFunction = this.httpClient.put.bind(this.httpClient)
		const patchFunction = this.httpClient.patch.bind(this.httpClient)
		const deleteFunction = this.httpClient.delete.bind(this.httpClient)
		this.httpClient.post = (endpoint, body, options) => {
			const [newBody, newHeader] = this.appendRequestToken(body, options?.headers)
			const newOptions = !options ? { headers: newHeader } : { ...options, headers: newHeader }
			return postFunction(endpoint, newBody, newOptions)
		}
		this.httpClient.put = (endpoint, body, options) => {
			const [newBody, newHeader] = this.appendRequestToken(body, options?.headers)
			const newOptions = !options ? { headers: newHeader } : { ...options, headers: newHeader }
			return putFunction(endpoint, newBody, newOptions)
		}
		this.httpClient.patch = (endpoint, body, options) => {
			const [newBody, newHeader] = this.appendRequestToken(body, options?.headers)
			const newOptions = !options ? { headers: newHeader } : { ...options, headers: newHeader }
			return patchFunction(endpoint, newBody, newOptions)
		}
		this.httpClient.delete = (endpoint, options) => {
			const newHeader = this.appendRequestToken(undefined, options?.headers)[1]
			const newOptions = !options ? { headers: newHeader } : { ...options, headers: newHeader }
			return deleteFunction(endpoint, newOptions)
		}
	}

	protected appendRequestToken(body?: HttpRequestBody, headers: HeadersInit = {}): [HttpRequestBody | undefined, HeadersInit] {
		if (this.csrfToken) {
			if (headers) {
				(headers as Record<string, string>)["X-CSRF-Token"] = this.csrfToken
			}
			if (body && body instanceof  FormData) {
				body.append("__CSRF-Token__", this.csrfToken)
			}
		}

		return [body, headers]
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
			this.setProgress(true, 1)
			return response.body as TResponse
		} finally {
			this.setProgress(false, 0)
		}
	}
}

export default ServiceBase
