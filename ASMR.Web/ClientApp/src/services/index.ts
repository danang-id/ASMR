
import ErrorCode from "@asmr/data/enumerations/ErrorCode"
import ResponseError from "@asmr/data/generic/ResponseError"
import { SetProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import { ILogger } from "@asmr/libs/common/logger"
import { INotificationHandler } from "@asmr/libs/common/notificationHandler"
import BeanService from "@asmr/services/BeanService"
import GateService from "@asmr/services/GateService"
import ProductService from "@asmr/services/ProductService"
import StatusService from "@asmr/services/StatusService"
import UserService from "@asmr/services/UserService"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"

export type Services = {
	abort: () => void
	handleError: (error?: Error, notification?: INotificationHandler, logger?: ILogger) => void
	handleErrors: (errors?: ResponseError[], notification?: INotificationHandler, logger?: ILogger) => void
	bean: BeanService
	gate: GateService
	product: ProductService
	status: StatusService
	user: UserService
}

export function createServices(controller: AbortController, setProgress?: SetProgressInfo): Services {
	function abort() {
		if (setProgress) {
			setProgress(false, 0)
		}
		controller.abort()
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

	function handleErrors(errors?: ResponseError[], notification?: INotificationHandler, logger?: ILogger) {
		function printError(message: string) {
			if (logger) {
				logger.error("Request returns error, reason:", message)
			}
			if (notification) {
				notification.error(message)
			}
		}

		if (errors && Array.isArray(errors)) {
			if (errors.findIndex(error => error.code === ErrorCode.NotAuthenticated) !== -1) {
				window.location.href = AuthenticationRoutes.SignOutPage + "?invalidSession=1"
			}
			for (const error of errors.reverse()) {
				switch (error.code) {
					case ErrorCode.InvalidAntiforgeryToken:
						printError("Your session has ended. Please refresh this page.")
						break;
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
		bean: new BeanService(controller, setProgress),
		gate: new GateService(controller, setProgress),
		product: new ProductService(controller, setProgress),
		status: new StatusService(controller, setProgress),
		user: new UserService(controller, setProgress)
	}
}

export default Services
