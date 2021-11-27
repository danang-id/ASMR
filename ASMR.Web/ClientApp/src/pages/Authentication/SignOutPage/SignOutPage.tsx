import { useEffect } from "react"
import { Navigate, useLocation } from "react-router-dom"
import BaseLayout from "asmr/layouts/BaseLayout"
import useAuthentication from "asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useNotification from "asmr/libs/hooks/notificationHook"
import "asmr/pages/Authentication/SignOutPage/SignOutPage.scoped.css"

function SignOutPage(): JSX.Element {
	useDocumentTitle("Sign Out")
	const authentication = useAuthentication()
	const location = useLocation()
	const logger = useLogger(SignOutPage)
	const notification = useNotification()
	const signInRedirectPath = `/authentication/sign-in${location.search}`

	async function signOut() {
		try {
			logger.info("Trying to sign-out...")
			const result = await authentication.signOut()
			if (result.isSuccess) {
				logger.info("Sign-out success, redirecting to:", signInRedirectPath)
				return
			}

			authentication.handleErrors(result.errors, notification, logger)
		} catch (error) {
			authentication.handleError(error as Error, notification, logger)
		}
	}

	useEffect(() => {
		if (authentication.isAuthenticated()) {
			signOut().then()
		}
	}, [])

	return !authentication.isAuthenticated() ? (
		<Navigate replace to={signInRedirectPath} />
	) : (
		<BaseLayout className="page">
			<p>Signing you out...</p>
		</BaseLayout>
	)
}

export default SignOutPage
