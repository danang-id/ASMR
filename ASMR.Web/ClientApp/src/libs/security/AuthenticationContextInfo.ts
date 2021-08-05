import Role from "@asmr/data/enumerations/Role"
import User from "@asmr/data/models/User"
import AuthenticationResponseModel from "@asmr/data/response/AuthenticationResponseModel"
import ResponseError from "@asmr/data/generic/ResponseError"
import { ILogger } from "@asmr/libs/common/logger"
import { INotificationHandler } from "@asmr/libs/common/notificationHandler"

export type AuthenticationContextInfo = {
	user?: User
	abort: () => void
	handleError: (error?: Error, notification?: INotificationHandler, logger?: ILogger) => void
	handleErrors: (errors?: ResponseError[], notification?: INotificationHandler, logger?: ILogger) => void
	isAuthenticated: () => boolean
	isAuthorized: (roles: Role[]) => boolean
	signIn: (username?: string, password?: string) => Promise<AuthenticationResponseModel>
	signOut: () => Promise<AuthenticationResponseModel>
	updateUserData: () => Promise<void>
}

export default AuthenticationContextInfo
