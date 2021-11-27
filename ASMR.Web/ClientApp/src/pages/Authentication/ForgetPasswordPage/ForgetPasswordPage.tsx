import { ChangeEvent, FormEvent, MouseEvent, useRef, useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import { IoHomeOutline, IoMail } from "react-icons/io5"
import ReCAPTCHA from "react-google-recaptcha"
import ApplicationLogo from "asmr/components/ApplicationLogo"
import Button from "asmr/components/Button"
import Form from "asmr/components/Form"
import BaseLayout from "asmr/layouts/BaseLayout"
import config from "asmr/libs/common/config"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useNotification from "asmr/libs/hooks/notificationHook"
import useProgress from "asmr/libs/hooks/progressHook"
import useServices from "asmr/libs/hooks/servicesHook"
import "asmr/pages/Authentication/ForgetPasswordPage/ForgetPasswordPage.scoped.css"

interface InstructionSentProps {
	emailAddress: string
}

function InstructionSent({ emailAddress }: InstructionSentProps): JSX.Element {
	const navigate = useNavigate()

	function onHomeButtonClicked() {
		navigate("/")
	}

	return (
		<BaseLayout className="page">
			<div className="header">
				<Link to="/"><ApplicationLogo /></Link>
				<p className="title">{config.application.name}</p>
			</div>
			<span className="separator" />
			<div className="description">
				<div>
					If we found an account with the email address <strong>{emailAddress}</strong>, we will send an
					instruction to reset your password.
				</div>
				<br />
				<div>Please check your email inbox and follow the instruction we sent you.</div>
			</div>
			<span className="separator" />
			<div className="call-to-action">
				<Button onClick={onHomeButtonClicked}>
					Home&nbsp;&nbsp;
					<IoHomeOutline />
				</Button>
			</div>
		</BaseLayout>
	)
}

function ForgetPasswordPage(): JSX.Element {
	useDocumentTitle("Forget Password")
	const [emailAddress, setEmailAddress] = useState("")
	const [resetInstructionSent, setResetInstructionSent] = useState(false)
	const recaptchaRef = useRef<ReCAPTCHA>(null)
	const logger = useLogger(ForgetPasswordPage)
	const navigate = useNavigate()
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	function onSignInButtonClicked() {
		navigate("/authentication/sign-in")
	}

	function onEmailAddressChanged(event: ChangeEvent<HTMLInputElement>) {
		setEmailAddress(event.target.value)
	}

	function onSendResetPasswordInstructionButtonClicked(event: MouseEvent<HTMLButtonElement>) {
		event.preventDefault()
		forgetPassword().then()
	}

	function onForgetPasswordFormSubmitted(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		forgetPassword().then()
	}

	async function forgetPassword() {
		if (progress.loading || !recaptchaRef || !recaptchaRef.current) {
			return
		}

		try {
			setResetInstructionSent(false)
			const result = await services.gate.forgetPassword({ emailAddress }, recaptchaRef.current.getValue())
			if (result.isSuccess) {
				setResetInstructionSent(true)
				return
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	if (resetInstructionSent) {
		return <InstructionSent emailAddress={emailAddress} />
	}

	return (
		<BaseLayout className="page">
			<div className="card">
				<div className="card-header">
					<Link to="/"><ApplicationLogo /></Link>
					<p>Forget Password</p>
				</div>
				<div className="card-body">
					<Form className="forget-password-form" onSubmit={onForgetPasswordFormSubmitted}>
						<div className="form-row">
							<div className="form-icon">
								<IoMail />
							</div>
							<Form.Input
								disabled={progress.loading}
								placeholder="Email Address"
								type="email"
								value={emailAddress}
								onChange={onEmailAddressChanged}
							/>
						</div>
						<div className="recaptcha-row">
							<ReCAPTCHA ref={recaptchaRef} sitekey={config.googleRecaptchaSiteKey} />
						</div>
						<div className="call-to-action">
							<Button
								className="send-reset-password-instruction-button"
								disabled={progress.loading}
								type="submit"
								onClick={onSendResetPasswordInstructionButtonClicked}
							>
								Send Reset Password Instruction
							</Button>
						</div>
						<div className="other-actions">
							<Button style="none" type="button" onClick={onSignInButtonClicked}>
								I remember my password
							</Button>
						</div>
					</Form>
				</div>
			</div>
		</BaseLayout>
	)
}

export default ForgetPasswordPage
