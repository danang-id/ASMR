import { ReactNode, useEffect, useState } from "react"
import ReactGA from "react-ga"
import Role from "@asmr/data/enumerations/Role";
import User from "@asmr/data/models/User"
import config from "@asmr/libs/common/config"
import useInit from "@asmr/libs/hooks/initHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import usePersistedState from "@asmr/libs/hooks/persistedStateHook"
import AuthenticationContext from "@asmr/libs/security/AuthenticationContext"
import ErrorCode from "@asmr/data/enumerations/ErrorCode"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"

interface AuthenticationProviderProps {
	children: ReactNode
	fallbackComponent?: JSX.Element
}

function AuthenticationProvider({children, fallbackComponent: fallback}: AuthenticationProviderProps): JSX.Element {
	useInit(onInit)
	const [initialized, setInitialized] = useState(false)
	const [user, setUser] = usePersistedState<User>("authentication.user")
	const services = useServices()

	function parseUserData(user: User): User {
		return { ...user, createdAt: new Date(user.createdAt), lastUpdatedAt: new Date(user.lastUpdatedAt) }
	}

	function isAuthenticated() {
		return !!user
	}

	function isAuthorized(roles: Role[]) {
		if (!user?.roles) {
			return false
		}

		if (!roles || !Array.isArray(roles)) {
			return false
		}

		for (const userRole of user.roles) {
			if (roles.findIndex(role => role === userRole.role) !== -1) {
				return true
			}
		}

		return false
	}

	async function onInit() {
		await updateUserData()
		setInitialized(true)
	}

	async function signIn(username?: string, password?: string, rememberMe?: boolean) {
		const result = await services.gate.authenticate({ username, password, rememberMe })
		if (result.isSuccess && result.data) {
			setUser(parseUserData(result.data))
		}

		return result
	}

	async function signOut() {
		const result = await services.gate.clearSession()
		if (result.isSuccess) {
			setUser(void 0)
		}
		return result
	}

	async function updateUserData() {
		try	{
			const result = await services.gate.getUserPassport()
			if (result.isSuccess && result.data) {
				setUser(parseUserData(result.data))
			}

			if (result.errors && Array.isArray(result.errors)) {
				const hasNotAuthenticatedError = result.errors
					.findIndex(error => error.code === ErrorCode.NotAuthenticated) !== -1
				if (!hasNotAuthenticatedError) {
					services.handleErrors(result.errors)
					return
				}

				if (isAuthenticated()) {
					window.location.href = AuthenticationRoutes.SignOutPage + "?invalidSession=1"
				}
			}
		} catch (error) {
			services.handleError(error as Error)
		}
	}

	useEffect(() => {
		if (config.googleAnalyticsMeasurementID) {
			ReactGA.set({ userId: user?.id ?? null, userName: user?.username ?? null })
		}
	}, [user])

	if (!initialized) {
		return fallback ?? <></>
	}

	return (
		<AuthenticationContext.Provider value={{
			user,
			abort: services.abort,
			handleError: services.handleError,
			handleErrors: services.handleErrors,
			isAuthenticated,
			isAuthorized,
			signIn,
			signOut,
			updateUserData
		}}>
			{children}
		</AuthenticationContext.Provider>
	)
}

export default AuthenticationProvider
