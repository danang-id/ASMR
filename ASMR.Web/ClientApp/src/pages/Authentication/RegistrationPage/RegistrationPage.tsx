import { ChangeEvent, FormEvent, MouseEvent, useEffect, useRef, useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import { IoHomeOutline, IoLogInOutline } from "react-icons/io5"
import ReCAPTCHA from "react-google-recaptcha"
import ApplicationLogo from "asmr/components/ApplicationLogo"
import Button from "asmr/components/Button"
import Form from "asmr/components/Form"
import ImageCropper from "asmr/components/ImageCropper"
import RegistrationRequestModel from "asmr/core/request/RegistrationRequestModel"
import BaseLayout from "asmr/layouts/BaseLayout"
import { getFileFromCanvas } from "asmr/libs/common/canvas"
import config from "asmr/libs/common/config"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import useInit from "asmr/libs/hooks/initHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useNotification from "asmr/libs/hooks/notificationHook"
import useProgress from "asmr/libs/hooks/progressHook"
import useServices from "asmr/libs/hooks/servicesHook"
import "asmr/pages/Authentication/RegistrationPage/RegistrationPage.scoped.css"

interface RegistrationDonePageProps {
	message?: string
}

function RegistrationDonePage({ message }: RegistrationDonePageProps): JSX.Element {
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
			<div className="description">{message}</div>
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

function RegistrationPage(): JSX.Element {
	const emptyRequestModel: RegistrationRequestModel = {
		firstName: "",
		lastName: "",
		emailAddress: "",
		emailAddressConfirmation: "",
		username: "",
		password: "",
		passwordConfirmation: "",
	}

	useDocumentTitle("Register")
	useInit(onInit)
	const [croppedImageFile, setCroppedImageFile] = useState<File | null>(null)
	const [imageBuffer, setImageBuffer] = useState<string | ArrayBuffer | null>(null)
	const [imageFile, setImageFile] = useState<File | null>(null)
	const [requestModel, setRequestModal] = useState<RegistrationRequestModel>(emptyRequestModel)
	const [registrationDone, setRegistrationDone] = useState(false)
	const [registrationMessage, setRegistrationMessage] = useState<string | undefined>()
	const recaptchaRef = useRef<ReCAPTCHA>(null)
	const logger = useLogger(RegistrationPage)
	const navigate = useNavigate()
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	function onInit() {
		setImageBuffer(null)
		setRequestModal(emptyRequestModel)
	}

	function onSignInButtonClicked() {
		navigate("/authentication/sign-in")
	}

	function onChange(event: ChangeEvent<HTMLInputElement>) {
		const newRequestModel = { ...requestModel }
		if (event.target.name === "image" && event.target.files) {
			setImageFile(event.target.files[0])
		} else {
			// @ts-ignore
			newRequestModel[event.target.name] = event.target.value
		}

		setRequestModal(newRequestModel)
	}

	function onUserImageCropped(cropper: Cropper) {
		const canvas = cropper.getCroppedCanvas()
		getFileFromCanvas(canvas, imageFile?.name ? imageFile.name : "", imageFile?.type).then(setCroppedImageFile)
	}

	function onRegisterButtonClicked(event: MouseEvent<HTMLButtonElement>) {
		event.preventDefault()
		register().then()
	}

	function onRegistrationFormSubmitted(event: FormEvent<HTMLFormElement>) {
		event.preventDefault()
		register().then()
	}

	async function register() {
		if (progress.loading || !recaptchaRef || !recaptchaRef.current) {
			return
		}

		try {
			setRegistrationDone(false)
			logger.info("Registering user", requestModel.username)
			const result = await services.gate.register(requestModel, croppedImageFile, recaptchaRef.current.getValue())
			if (result.isSuccess) {
				setRegistrationMessage(result.message)
				setRegistrationDone(true)
				return
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	useEffect(() => {
		if (!imageFile) {
			setImageBuffer(null)
			return
		}

		const fileReader = new FileReader()
		fileReader.onloadend = () => {
			setImageBuffer(fileReader.result)
		}
		fileReader.readAsDataURL(imageFile)
	}, [imageFile])

	if (registrationDone) {
		return <RegistrationDonePage message={registrationMessage} />
	}

	return (
		<BaseLayout className="page">
			<div className="card">
				<div className="card-header">
					<Link to="/"><ApplicationLogo /></Link>
					<p>Register</p>
				</div>
				<div className="card-body">
					<Form className="registration-form" onSubmit={onRegistrationFormSubmitted}>
						<div className="form-row">
							<label className="form-field">First Name</label>
							<div className="form-data">
								<Form.Input name="firstName" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Last Name</label>
							<div className="form-data">
								<Form.Input name="lastName" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Email Address</label>
							<div className="form-data">
								<Form.Input name="emailAddress" type="email" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Confirm Email Address</label>
							<div className="form-data">
								<Form.Input name="emailAddressConfirmation" type="email" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Username</label>
							<div className="form-data">
								<Form.Input name="username" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Password</label>
							<div className="form-data">
								<Form.Input name="password" type="password" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Confirm Password</label>
							<div className="form-data">
								<Form.Input name="passwordConfirmation" type="password" onChange={onChange} />
							</div>
						</div>
						<div className="form-row">
							<label className="form-field">Image</label>
							<div className="form-data">
								<Form.Input
									accept="image/png, image/jpeg, image/webp"
									name="image"
									type="file"
									onChange={onChange}
								/>
							</div>
						</div>
						{imageBuffer && (
							<div className="form-row image-preview">
								<ImageCropper alt="User Image" source={imageBuffer} onCropped={onUserImageCropped} />
							</div>
						)}
						<div className="recaptcha-row">
							<ReCAPTCHA ref={recaptchaRef} sitekey={config.googleRecaptchaSiteKey} />
						</div>
						<div className="call-to-action">
							<Button
								className="register-button"
								disabled={progress.loading}
								icon={IoLogInOutline}
								type="submit"
								onClick={onRegisterButtonClicked}
							>
								Register
							</Button>
						</div>
						<div className="other-actions">
							<Button style="none" type="button" onClick={onSignInButtonClicked}>
								I have an account
							</Button>
						</div>
					</Form>
				</div>
			</div>
		</BaseLayout>
	)
}

export default RegistrationPage
