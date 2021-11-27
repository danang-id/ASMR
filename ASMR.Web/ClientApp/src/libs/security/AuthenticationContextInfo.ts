import Role from "asmr/core/enums/Role"
import User from "asmr/core/entities/User"
import AuthenticationResponseModel from "asmr/core/response/AuthenticationResponseModel"
import ErrorInformation from "asmr/core/common/ErrorInformation"
import { ILogger } from "asmr/libs/common/logger"
import { INotificationHandler } from "asmr/libs/common/notificationHandler"

export type AuthenticationContextInfo = {
	user?: User
	abort: () => void
	handleError: (error?: Error, notification?: INotificationHandler, logger?: ILogger) => void
	handleErrors: (errors?: ErrorInformation[], notification?: INotificationHandler, logger?: ILogger) => void
	isAuthenticated: () => boolean
	isAuthorized: (roles: Role[]) => boolean
	signIn: (username?: string, password?: string, rememberMe?: boolean) => Promise<AuthenticationResponseModel>
	signOut: () => Promise<AuthenticationResponseModel>
	updateUserData: () => Promise<void>
}

export default AuthenticationContextInfo
