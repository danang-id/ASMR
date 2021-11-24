import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse, CancelTokenSource } from "axios"
import Platform from "@asmr/data/enumerations/Platform"
import { SetProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import config from "@asmr/libs/common/config"
import { ILogger } from "@asmr/libs/common/logger"
import useLogger from "@asmr/libs/hooks/loggerHook"

export interface ServiceLogOptions {
	requestHeader?: boolean
	requestBody?: boolean
	responseHeader?: boolean
	responseBody?: boolean
}

export interface ServiceOptions {
	log: ServiceLogOptions
}

class ServiceBase {
	protected csrfToken?: string
	protected readonly httpClient: AxiosInstance
	protected readonly logger: ILogger
	protected readonly options: ServiceOptions
	protected readonly setProgress: SetProgressInfo

	constructor(cancelTokenSource: CancelTokenSource, options?: ServiceOptions, setProgress?: SetProgressInfo) {
		this.httpClient = axios.create({
			baseURL: config.backEndBaseUrl ?? window.location.origin,
			cancelToken: cancelTokenSource.token,
			headers: {
				Accept: "application/json",
			},
			params: {
				clientPlatform: Platform.Web,
				clientVersion: config.application.version,
			},
			validateStatus: (status) => status >= 200 && status <= 500,
			withCredentials: true,
			xsrfCookieName: "ASMR.CSRF-Request-Token",
			xsrfHeaderName: "X-CSRF-Token",
		})
		this.logger = useLogger(ServiceBase)
		this.options = options ?? {
			log: {
				requestHeader: false,
				requestBody: false,
				responseHeader: false,
				responseBody: false,
			},
		}
		this.setProgress = setProgress ?? (() => {})

		this.httpClient.interceptors.request.use(this.onRequestFulfilled.bind(this), this.onRequestRejected.bind(this))
		this.httpClient.interceptors.response.use(
			this.onResponseFulfilled.bind(this),
			this.onResponseRejected.bind(this)
		)
	}

	protected logRequest(request: AxiosRequestConfig, response: AxiosResponse) {
		this.logger.info(`[${response.status}] ${request.method?.toUpperCase()} ${request.url}`)
		if (this.options.log.requestHeader === true) {
			this.logger.info(request.headers)
		}
		if (this.options.log.requestBody === true && request.data) {
			this.logger.info(`Request: ${request.data}`)
		}
		if (this.options.log.responseHeader === true) {
			this.logger.info(response.headers)
		}
		if (this.options.log.responseBody === true && response.data) {
			this.logger.info(`Response: ${JSON.stringify(response.data)}`)
		}
	}

	protected async onRequestFulfilled(request: AxiosRequestConfig) {
		this.setProgress(true, 0)
		return request
	}

	protected onRequestRejected(error: Error) {
		this.logger.error(error)
		return Promise.reject(error)
	}

	protected async onResponseFulfilled(response: AxiosResponse) {
		this.logRequest(response.config, response)
		return response
	}

	protected onResponseRejected(error: Error) {
		this.logger.error(error)
		return Promise.reject(error)
	}

	protected processResponse<T>(response: AxiosResponse<T>): T {
		this.setProgress(true, 1)
		return response.data
	}

	protected finalize() {
		this.setProgress(false, 0)
	}
}

export default ServiceBase
