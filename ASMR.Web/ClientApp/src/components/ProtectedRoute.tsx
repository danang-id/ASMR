import { Component, ComponentType, ContextType } from "react"
import { RouteComponentProps } from "react-router"
import { Redirect, Route, RouteProps } from "react-router-dom"
import Role from "@asmr/data/enumerations/Role"
import Logger, { ILogger } from "@asmr/libs/common/logger"
import NotificationHandler, { INotificationHandler } from "@asmr/libs/common/notificationHandler"
import AuthenticationContext from "@asmr/libs/security/AuthenticationContext"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"

interface ProtectedRouteProps extends RouteProps {
	allowedRoles?: Role[]
	component: ComponentType<RouteComponentProps<any>> | ComponentType<any>
}

interface ProtectedRouteState {}

class ProtectedRoute extends Component<ProtectedRouteProps, ProtectedRouteState> {
	context!: ContextType<typeof AuthenticationContext>
	private readonly logger: ILogger
	private readonly notification: INotificationHandler

	constructor(props: ProtectedRouteProps) {
		super(props)
		this.logger = new Logger(ProtectedRoute.name)
		this.notification = new NotificationHandler()
	}

	static contextType = AuthenticationContext

	private renderRoute(props: RouteComponentProps) {
		const {
			allowedRoles = [Role.Administrator, Role.Roaster, Role.Server],
			component: Component
		} = this.props
		const authentication = this.context
		const state = { from: props.location }
		const urlSearchParams = new URLSearchParams()
		if (state.from) {
			urlSearchParams.append("next", state.from.pathname)
		}

		if (!authentication.isAuthenticated()) {
			// TODO: Authentication state seems to be updated late in ProtectedRoute component.
			// 		 This makes ProtectedRoute component detect that user needs authentication
			// 		 even right after user sign in.
			// 		 Workaround: warning notification disabled for now.
			this.logger.warn("Authentication required, redirecting to:", AuthenticationRoutes.SignInPage)
			// this.notification.warn("You are not authenticated. Please sign-in first.")
			return <Redirect to={{
				pathname: AuthenticationRoutes.SignInPage,
				search: urlSearchParams.toString(),
				state: state
			}} />
		}

		if (authentication.user && (!Array.isArray(authentication.user.roles) || authentication.user.roles.length <= 0)) {
			this.logger.warn("User authenticated, but no roles assigned, redirecting to:", AuthenticationRoutes.SignOutPage)
			this.notification.warn("You cannot access the dashboard because you have no roles assigned to you by the Administrator.")
			return <Redirect to={{ pathname: AuthenticationRoutes.SignOutPage, state }} />
		}

		if (!authentication.isAuthorized(allowedRoles)) {
			this.logger.warn("Not authorized, required roles:", allowedRoles.map(allowedRole => Role[allowedRole]))
			this.notification.warn("You are not authorized to this this resource.")
			return <Redirect to={{ pathname: DashboardRoutes.IndexPage, state }} />
		}

		return <Component {...props} />
	}

	public render() {
		// eslint-disable-next-line @typescript-eslint/no-unused-vars,no-unused-vars
		const { allowedRoles, component, ...routeProps } = this.props
		return <Route {...routeProps} render={this.renderRoute.bind(this)} />
	}
}

export default ProtectedRoute
