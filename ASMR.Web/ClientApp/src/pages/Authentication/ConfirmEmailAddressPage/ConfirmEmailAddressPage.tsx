import { useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import { IoCheckmarkOutline, IoCloseOutline, IoCogOutline, IoEnterOutline, IoRefreshOutline } from "react-icons/io5"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import BaseLayout from "@asmr/layouts/BaseLayout"
import config from "@asmr/libs/common/config"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import "@asmr/pages/Authentication/ConfirmEmailAddressPage/ConfirmEmailAddressPage.scoped.css"

function ConfirmEmailAddressPage(): JSX.Element {
	useDocumentTitle("Confirm Your Email Address")
	useInit(onInit)
	const [confirmationDone, setConfirmationDone] = useState(false)
	const [confirmationMessage, setConfirmationMessage] = useState<string | undefined>()
	const [emailConfirmed, setEmailConfirmed] = useState(false)
	const location = useLocation()
	const logger = useLogger(ConfirmEmailAddressPage)
	const navigate = useNavigate()
	const [progress] = useProgress()
	const services = useServices()

	const urlSearchParam = new URLSearchParams(location.search)
	const id = urlSearchParam.get("id")
	const token = urlSearchParam.get("token")
	const emailAddress = urlSearchParam.get("emailAddress")

	async function onInit() {
		await confirmEmailAddress()
	}

	function onSignInButtonClicked() {
		navigate("/authentication/sign-in")
	}

	function onTryAgainButtonClicked() {
		confirmEmailAddress().then()
	}

	async function confirmEmailAddress() {
		try {
			setConfirmationDone(false)
			setConfirmationMessage("Confirming your email address...")

			if (!id || !emailAddress || !token) {
				setConfirmationMessage("Your email address confirmation link is invalid.")
				return
			}

			const result = await services.gate.confirmEmailAddress({ id, emailAddress, token })
			if (result.isSuccess) {
				setConfirmationMessage(result.message)
				setEmailConfirmed(true)
				return
			}

			if (result.errors && result.errors[0]) {
				setEmailConfirmed(false)
				setConfirmationMessage(result.errors[0].reason)
				services.handleErrors(result.errors, void 0, logger)
			}
		} catch (error) {
			setEmailConfirmed(false)
			setConfirmationMessage((error as Error).message)
			services.handleError(error as Error, void 0, logger)
		} finally {
			setConfirmationDone(true)
		}
	}

	return (
		<BaseLayout className="page">
			<div className="header">
				<ApplicationLogo />
				<p className="title">{config.application.name}</p>
			</div>
			<span className="separator" />
			<div className="description">
				{progress.loading || !confirmationDone ? (
					<IoCogOutline className="animate-spin" />
				) : emailConfirmed ? (
					<IoCheckmarkOutline />
				) : (
					<IoCloseOutline />
				)}
				&nbsp;&nbsp;
				{confirmationMessage}
			</div>
			<span className="separator" />
			<div className="call-to-action">
				{!progress.loading &&
					confirmationDone &&
					(emailConfirmed ? (
						<Button onClick={onSignInButtonClicked}>
							Sign In&nbsp;&nbsp;
							<IoEnterOutline />
						</Button>
					) : (
						<Button onClick={onTryAgainButtonClicked}>
							Try Again&nbsp;&nbsp;
							<IoRefreshOutline />
						</Button>
					))}
			</div>
		</BaseLayout>
	)
}

export default ConfirmEmailAddressPage
