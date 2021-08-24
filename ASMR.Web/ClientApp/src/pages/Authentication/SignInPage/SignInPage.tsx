import { ChangeEvent, FormEvent, MouseEvent, useState } from "react"
import { Redirect, useHistory, useLocation } from "react-router-dom"
import { IoKey, IoLogInOutline, IoPerson } from "react-icons/io5"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import BaseLayout from "@asmr/layouts/BaseLayout"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import RegistrationModal from "@asmr/pages/Authentication/SignInPage/RegistrationModal"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import "@asmr/pages/Authentication/SignInPage/SignInPage.scoped.css"

function SignInPage(): JSX.Element {
	useDocumentTitle("Sign In")
	useInit(onInit)
	const [registrationModalShown, setRegistrationModalShown] = useState(false)
	const [nextUrl, setNextUrl] = useState<string>(DashboardRoutes.IndexPage)
	const [username, setUsername] = useState("")
	const [password, setPassword] = useState("")
	const [rememberMe, setRememberMe] = useState(false)
	const [signInExecuted, setSignInExecuted] = useState(false)
	const authentication = useAuthentication()
	const history = useHistory()
	const location = useLocation()
	const logger = useLogger(SignInPage)
	const notification = useNotification()
	const [progress] = useProgress()

	function onInit() {
		const urlSearchParams = new URLSearchParams(location.search)
		const invalidSessionParam = urlSearchParams.get("invalidSession")
		if (invalidSessionParam === "1") {
			notification.info("Your session has ended. Please sign in to continue.")
		}

		const nextParam = urlSearchParams.get("next")
		if (nextParam) {
			setNextUrl(nextParam)
		}
	}

	function onCloseModals() {
		setRegistrationModalShown(false)
	}

	function onShowRegistrationModalButtonClicked() {
		setRegistrationModalShown(true)
	}

	function onUsernameChanged(event: ChangeEvent<HTMLInputElement>) {
		setUsername(event.target.value)
	}

	function onPasswordChanged(event: ChangeEvent<HTMLInputElement>) {
		setPassword(event.target.value)
	}
	
	function onRememberMeChanged(event: ChangeEvent<HTMLInputElement>) {
		setRememberMe(event.target.checked)
	}

	function onSignInButtonClicked(event: MouseEvent<HTMLButtonElement>) {
		event.preventDefault()
		signIn().then()
	}

	function onSignInFormSubmit(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		signIn().then()
	}

	async function signIn() {
		if (progress.loading) {
			return
		}

		try {
			setSignInExecuted(true)
			logger.info("Trying to sign-in with username:", username.trim())
			const result = await authentication.signIn(username.trim(), password, rememberMe)
			if (result.isSuccess) {
				logger.info("Sign-in success, redirecting to:", nextUrl)
				history.push(nextUrl)
				return
			}

			authentication.handleErrors(result.errors, notification, logger)
		} catch (error) {
			authentication.handleError(error, notification, logger)
		}
	}

	if (!signInExecuted && authentication.isAuthenticated()) {
		return <Redirect to={nextUrl} />
	}

	return (
		<BaseLayout className="page">
			<div className="card">
				<div className="card-header">
					<ApplicationLogo />
					<p>Sign In</p>
				</div>
				<div className="card-body">
					<Form className="sign-in-form" onSubmit={onSignInFormSubmit} >
						<div className="form-row">
							<div className="form-icon">
								<IoPerson />
							</div>
							<Form.Input disabled={progress.loading}
								placeholder="Username"
								type="text"
								value={username}
								onChange={onUsernameChanged} />
						</div>
						<div className="form-row">
							<div className="form-icon">
								<IoKey />
							</div>
							<Form.Input disabled={progress.loading}
								placeholder="Password"
								type="password"
								value={password}
								onChange={onPasswordChanged} />
						</div>
						<div className="form-row">
							<div className="remember-me">
								<Form.CheckBox checked={rememberMe}
									disabled={progress.loading}
									onChange={onRememberMeChanged}>Remember Me?</Form.CheckBox>
							</div>
						</div>
						<div className="call-to-action">
							<Button style="none" 
									type="button"
									onClick={onShowRegistrationModalButtonClicked}>
								I don't have an account
							</Button>
							<Button className="sign-in-button" 
									disabled={progress.loading} 
									icon={IoLogInOutline}
									type="submit" 
									onClick={onSignInButtonClicked}>
								Sign In
							</Button>
						</div>
					</Form>
				</div>
			</div>
			
			<RegistrationModal onClose={onCloseModals} show={registrationModalShown} />
		</BaseLayout>
	)
}

export default SignInPage
