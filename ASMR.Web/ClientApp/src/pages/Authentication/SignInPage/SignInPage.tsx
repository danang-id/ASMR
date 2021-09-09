import { ChangeEvent, FormEvent, MouseEvent, useState } from "react"
import { Redirect, useHistory, useLocation } from "react-router-dom"
import { IoHomeOutline, IoKey, IoLogInOutline, IoPerson } from "react-icons/io5"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import ErrorCode from "@asmr/data/enumerations/ErrorCode"
import BaseLayout from "@asmr/layouts/BaseLayout"
import config from "@asmr/libs/common/config"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"
import "@asmr/pages/Authentication/SignInPage/SignInPage.scoped.css"
import useServices from "@asmr/libs/hooks/servicesHook"

interface AccountProblemPageProps {
	code: ErrorCode
	description: string
	username: string
	password: string
}

function AccountProblemPage({ code, description, username, password }: AccountProblemPageProps): JSX.Element {
	const [resendDone, setResendDone] = useState(false)
	const history = useHistory()
	const logger = useLogger(AccountProblemPage)
	const notification = useNotification()
	const services = useServices()

	function onHomeButtonClicked() {
		history.push(PublicRoutes.HomePage)
	}

	function onResendEmailAddressConfirmationButtonClicked() {
		resendEmailAddressConfirmation().then()
	}

	async function resendEmailAddressConfirmation() {
		try {
			const result = await services.gate.resendEmailAddressConfirmation({ username, password })
			if (result.isSuccess) {
				notification.success(result.message ??
					"An email has been sent to the email address registered for your account. " +
					"Please check your email inbox.")
				setResendDone(true)
				return
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	return (
		<BaseLayout className="page">
			<div className="header">
				<ApplicationLogo/>
				<p className="title">{config.application.name}</p>
			</div>
			<span className="separator" />
			<div className="description">
				{ description }
			</div>
			<span className="separator" />
			<div className="call-to-action">
				{(code === ErrorCode.EmailAddressWaitingConfirmation && !resendDone) && (
					<Button style="outline"
							onClick={onResendEmailAddressConfirmationButtonClicked}>
						Resend Confirmation Email
					</Button>
				)}
				<Button onClick={onHomeButtonClicked}>
					Home&nbsp;&nbsp;<IoHomeOutline />
				</Button>
			</div>
		</BaseLayout>
	)
}

function SignInPage(): JSX.Element {
	useDocumentTitle("Sign In")
	useInit(onInit)
	const [accountProblem, setAccontProblem] = useState<AccountProblemPageProps | null>(null)
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

	function onRegisterButtonClicked() {
		history.push(AuthenticationRoutes.RegistrationPage)
	}

	function onForgetPasswordButtonClicked() {
		history.push(AuthenticationRoutes.ForgetPasswordPage)
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

	function onSignInFormSubmitted(event: FormEvent<HTMLFormElement>) {
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

			if (result.errors && result.errors[0]) {
				const error = result.errors[0]
				const showAccountProblemPage = error.code === ErrorCode.EmailAddressWaitingConfirmation ||
					error.code === ErrorCode.AccountWaitingForApproval ||
					error.code === ErrorCode.AccountWasNotApproved

				if (showAccountProblemPage) {
					setAccontProblem({
						code: error.code,
						description: error.reason,
						username: username.trim(),
						password: password
					})
					return
				}
			}

			authentication.handleErrors(result.errors, notification, logger)
		} catch (error) {
			authentication.handleError(error as Error, notification, logger)
		}
	}

	if (!signInExecuted && authentication.isAuthenticated()) {
		return <Redirect to={nextUrl} />
	}

	if (accountProblem) {
		return <AccountProblemPage code={accountProblem.code}
									description={accountProblem.description}
									username={accountProblem.username}
									password={accountProblem.password} />
	}

	return (
		<BaseLayout className="page">
			<div className="card">
				<div className="card-header">
					<ApplicationLogo />
					<p>Sign In</p>
				</div>
				<div className="card-body">
					<Form className="sign-in-form" onSubmit={onSignInFormSubmitted} >
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
							<Button className="sign-in-button"
									disabled={progress.loading}
									icon={IoLogInOutline}
									type="submit"
									onClick={onSignInButtonClicked}>
								Sign In
							</Button>
						</div>
						<div className="other-actions">
							<Button style="none"
									type="button"
									onClick={onRegisterButtonClicked}>
								I don't have an account
							</Button>
							<Button style="none"
									type="button"
									onClick={onForgetPasswordButtonClicked}>
								I forget my password
							</Button>
						</div>
					</Form>
				</div>
			</div>
		</BaseLayout>
	)
}

export default SignInPage
