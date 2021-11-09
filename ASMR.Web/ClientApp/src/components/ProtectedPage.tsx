import { Navigate, useLocation } from "react-router-dom"
import Role from "@asmr/data/enumerations/Role"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import { ReactNode } from "react"

interface ProtectedRouteProps {
	allowedRoles?: Role[]
	children: ReactNode
}

function ProtectedPage(props: ProtectedRouteProps): JSX.Element {
	const authentication = useAuthentication()
	const location = useLocation()
	const logger = useLogger(ProtectedPage)
	const notification = useNotification()

	const { allowedRoles = [Role.Administrator, Role.Roaster, Role.Server] } = props
	const state = { from: location }
	const urlSearchParams = new URLSearchParams()
	urlSearchParams.append("next", state.from.pathname)

	if (!authentication.isAuthenticated()) {
		// TODO: Authentication state seems to be updated late in ProtectedRoute component.
		// 		 This makes ProtectedRoute component detect that user needs authentication
		// 		 even right after user sign in.
		// 		 Workaround: warning notification disabled for now.
		logger.warn("Authentication required, redirecting to:", "/authentication/sign-in")
		// notification.warn("You are not authenticated. Please sign-in first.")
		return (
			<Navigate
				replace
				state={state}
				to={{
					pathname: "/authentication/sign-in",
					search: urlSearchParams.toString()
				}}
			/>
		)
	}

	if (authentication.user && (!Array.isArray(authentication.user.roles) || authentication.user.roles.length <= 0)) {
		logger.warn("User authenticated, but no roles assigned, redirecting to:", "/authentication/sign-out")
		notification.warn(
			"You cannot access the dashboard because you have no roles assigned to you by the Administrator."
		)
		return <Navigate replace state={state} to={{ pathname: "/authentication/sign-out" }} />
	}

	if (!authentication.isAuthorized(allowedRoles)) {
		logger.warn("Not authorized, required roles:", allowedRoles.map((allowedRole) => Role[allowedRole]))
		notification.warn("You are not authorized to this this resource.")
		return <Navigate replace state={state} to={{ pathname: "/dashboard" }} />
	}

	return props.children as JSX.Element
}

export default ProtectedPage
