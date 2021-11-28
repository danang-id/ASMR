import axios from "axios"
import ErrorCode from "asmr/core/enums/ErrorCode"
import ErrorInformation from "asmr/core/common/ErrorInformation"
import { SetProgressInfo } from "asmr/libs/application/ProgressContextInfo"
import { ILogger } from "asmr/libs/common/logger"
import { INotificationHandler } from "asmr/libs/common/notificationHandler"
import BeanService from "asmr/services/BeanService"
import BusinessAnalyticService from "asmr/services/BusinessAnalyticService"
import GateService from "asmr/services/GateService"
import ProductService from "asmr/services/ProductService"
import ReleaseService from "asmr/services/ReleaseService"
import StatusService from "asmr/services/StatusService"
import UserService from "asmr/services/UserService"

export type Services = {
	abort: (message?: string) => void
	handleError: (error?: Error, notification?: INotificationHandler, logger?: ILogger) => void
	handleErrors: (errors?: ErrorInformation[], notification?: INotificationHandler, logger?: ILogger) => void
	bean: BeanService
	businessAnalytic: BusinessAnalyticService
	gate: GateService
	product: ProductService
	release: ReleaseService
	status: StatusService
	user: UserService
}

export function createServices(setProgress?: SetProgressInfo): Services {
	const cancelTokenSource = axios.CancelToken.source()

	function abort(message?: string) {
		if (setProgress) {
			setProgress(false, 0)
		}
		cancelTokenSource.cancel(message)
	}

	function handleError(error?: Error, notification?: INotificationHandler, logger?: ILogger) {
		if (error && error.message) {
			if (error.message === "Failed to fetch") {
				error.message = "We are unable to communicate with our back at the moment. Please try again later."
			}
			if (logger) {
				logger.error("Caught error, message:", error.message)
			}
			if (notification) {
				notification.error(error.message)
			}
		}
	}

	function handleErrors(errors?: ErrorInformation[], notification?: INotificationHandler, logger?: ILogger) {
		function printError(message: string) {
			if (logger) {
				logger.error("Request returns error, reason:", message)
			}
			if (notification) {
				notification.error(message)
			}
		}

		if (errors && Array.isArray(errors)) {
			if (errors.findIndex((error) => error.code === ErrorCode.NotAuthenticated) !== -1) {
				window.location.href = "/authentication/sign-out?invalidSession=1"
			}
			for (const error of errors.reverse()) {
				switch (error.code) {
					case ErrorCode.InvalidAntiforgeryToken:
						printError("Your session has ended. Please refresh this page.")
						break
					default:
						printError(error.reason)
				}
			}
		}
	}

	return {
		abort,
		handleError,
		handleErrors,
		bean: new BeanService(cancelTokenSource, void 0, setProgress),
		businessAnalytic: new BusinessAnalyticService(cancelTokenSource, void 0, setProgress),
		gate: new GateService(cancelTokenSource, void 0, setProgress),
		product: new ProductService(cancelTokenSource, void 0, setProgress),
		release: new ReleaseService(cancelTokenSource, void 0, setProgress),
		status: new StatusService(cancelTokenSource, void 0, setProgress),
		user: new UserService(cancelTokenSource, void 0, setProgress),
	}
}

export default Services
