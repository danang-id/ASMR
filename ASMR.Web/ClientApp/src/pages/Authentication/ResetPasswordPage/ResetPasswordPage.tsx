import { ChangeEvent, FormEvent, MouseEvent, useState } from "react"
import { useHistory, useLocation } from "react-router-dom"
import { IoEnterOutline, IoHomeOutline, IoKey, IoMail } from "react-icons/io5"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import BaseLayout from "@asmr/layouts/BaseLayout"
import config from "@asmr/libs/common/config"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"
import "@asmr/pages/Authentication/ResetPasswordPage/ResetPasswordPage.scoped.css"

interface ResetPasswordDoneProps {
	message?: string
	success?: boolean
}

function ResetPasswordDone({ message, success = false }: ResetPasswordDoneProps): JSX.Element {
	const history = useHistory()

	function onHomeButtonClicked() {
		history.push(PublicRoutes.HomePage)
	}

	function onSignInButtonClicked() {
		history.push(AuthenticationRoutes.SignInPage)
	}

	return (
		<BaseLayout className="page">
			<div className="header">
				<ApplicationLogo/>
				<p className="title">{config.application.name}</p>
			</div>
			<span className="separator" />
			<div className="description">
				{ message }
			</div>
			<span className="separator" />
			<div className="call-to-action">
				{ success ? (
					<Button onClick={onSignInButtonClicked}>
						Sign In&nbsp;&nbsp;<IoEnterOutline />
					</Button>
				) : (
					<Button onClick={onHomeButtonClicked}>
						Home&nbsp;&nbsp;<IoHomeOutline />
					</Button>
				)}
			</div>
		</BaseLayout>
	)
}

function ResetPasswordPage(): JSX.Element {
	useDocumentTitle("Reset Password")
	const [password, setPassword] = useState("")
	const [passwordConfirmation, setPasswordConfirmation] = useState("")
	const [resetPasswordDone, setResetPasswordDone] = useState(false)
	const [resetPasswordMessage, setResetPasswordMessage] = useState<string | undefined>(
		"Your reset password link is invalid.")
	const location = useLocation()
	const logger = useLogger(ResetPasswordPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	const urlSearchParam = new URLSearchParams(location.search)
	const id = urlSearchParam.get("id")
	const token = urlSearchParam.get("token")
	const emailAddress = urlSearchParam.get("emailAddress")

	function onPasswordChanged(event: ChangeEvent<HTMLInputElement>) {
		setPassword(event.target.value)
	}

	function onPasswordConfirmationChanged(event: ChangeEvent<HTMLInputElement>) {
		setPasswordConfirmation(event.target.value)
	}

	function onResetPasswordButtonClicked(event: MouseEvent<HTMLButtonElement>) {
		event.preventDefault()
		resetPassword().then()
	}

	function onResetPasswordFormSubmitted(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		resetPassword().then()
	}

	async function resetPassword() {
		if (progress.loading || !id || !emailAddress || !token) {
			return
		}

		setResetPasswordDone(false)

		try {
			const result = await services.gate.resetPassword({
				id, emailAddress, token, password, passwordConfirmation
			})
			if (result.isSuccess) {
				setResetPasswordMessage(result.message)
				setResetPasswordDone(true)
				return
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	if (!id || !emailAddress || !token) {
		return <ResetPasswordDone message={resetPasswordMessage} />
	}

	if (resetPasswordDone) {
		return <ResetPasswordDone success message={resetPasswordMessage} />
	}

	return (
		<BaseLayout className="page">
			<div className="card">
				<div className="card-header">
					<ApplicationLogo />
					<p>Reset Password</p>
				</div>
				<div className="card-body">
					<Form className="reset-password-form" onSubmit={onResetPasswordFormSubmitted} >
						<div className="form-row">
							<div className="form-icon">
								<IoMail />
							</div>
							<Form.Input disabled readOnly
										placeholder="Email Address"
										type="email"
										value={emailAddress} />
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
							<div className="form-icon">
								<IoKey />
							</div>
							<Form.Input disabled={progress.loading}
										placeholder="Confirm Password"
										type="password"
										value={passwordConfirmation}
										onChange={onPasswordConfirmationChanged} />
						</div>
						<div className="call-to-action">
							<Button className="reset-password-button"
									disabled={progress.loading}
									type="submit"
									onClick={onResetPasswordButtonClicked}>
								Reset Password
							</Button>
						</div>
					</Form>
				</div>
			</div>
		</BaseLayout>
	)
}

export default ResetPasswordPage
