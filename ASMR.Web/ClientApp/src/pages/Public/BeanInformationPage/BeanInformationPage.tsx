import { useEffect, useRef, useState } from "react"
import { useParams } from "react-router-dom"
import { IoPrintOutline } from "react-icons/io5"
import { useReactToPrint } from "react-to-print"
import BeanDescription from "asmr/components/BeanDescription"
import Button from "asmr/components/Button"
import QuickResponseCode from "asmr/components/QuickResponseCode"
import BeanImage from "asmr/components/BeanImage"
import Bean from "asmr/core/entities/Bean"
import BaseLayout from "asmr/layouts/BaseLayout"
import config from "asmr/libs/common/config"
import { getBaseUrl } from "asmr/libs/common/location"
import useAuthentication from "asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useNotification from "asmr/libs/hooks/notificationHook"
import useProgress from "asmr/libs/hooks/progressHook"
import useServices from "asmr/libs/hooks/servicesHook"
import "asmr/pages/Public/BeanInformationPage/BeanInformationPage.scoped.css"

function BeanInformationPage(): JSX.Element {
	useDocumentTitle("Bean Information")
	const { beanId } = useParams<"beanId">()
	const [bean, setBean] = useState<Bean | null>(null)
	const [initialized, setInitialized] = useState(false)
	const [quickResponseCodeValue, setQuickResponseCodeValue] = useState("")
	const authentication = useAuthentication()
	const logger = useLogger(BeanInformationPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	const printComponentRef = useRef<HTMLDivElement>(null)
	const handlePrintComponent = useReactToPrint({
		content: () => printComponentRef.current,
	})

	function onPrintButtonClicked() {
		if (handlePrintComponent) {
			handlePrintComponent()
		}
	}

	async function retrieveGreenBeanInformation() {
		try {
			const result = await services.bean.getById(beanId ?? "")
			if (result.isSuccess && result.data) {
				const url = new URL("/pub/bean/" + result.data.id, getBaseUrl())
				setBean(result.data)
				setQuickResponseCodeValue(url.toString())
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		} finally {
			setInitialized(true)
		}
	}

	useEffect(() => {
		if (beanId) {
			retrieveGreenBeanInformation().then()
		}
	}, [beanId])

	if (progress.loading || !initialized) {
		return (
			<BaseLayout>
				<div className="container">
					<div className="content">
						<div className="information">
							<div className="bean">
								<div className="name">Retrieving information...</div>
							</div>
						</div>
					</div>
				</div>
			</BaseLayout>
		)
	}

	return (
		<BaseLayout>
			<div className="container" ref={printComponentRef}>
				<div className="content">
					<div className="top-container">
						{quickResponseCodeValue && (
							<div className="qr-code">
								<div className="qr-code-image">
									<QuickResponseCode value={quickResponseCodeValue} />
								</div>
							</div>
						)}
						<div className="information">
							<div className="bean">
								<div className="name">{bean ? bean.name : "The bean does not exist"}</div>
								<div className="description">
									<BeanDescription description={bean?.description} />
								</div>
							</div>
						</div>
					</div>
					<div className="about">
						<hr />
						<div>Bean Information</div>
						<div>
							<strong>{config.application.name}</strong>
						</div>
					</div>
				</div>
			</div>
			{bean && (
				<div className="bean-detail">
					<div className="bean-image-container">
						<BeanImage bean={bean} />
					</div>
					{authentication.isAuthenticated() && (
						<div className="actions">
							<Button onClick={onPrintButtonClicked} icon={IoPrintOutline}>
								Print
							</Button>
						</div>
					)}
				</div>
			)}
		</BaseLayout>
	)
}

export default BeanInformationPage
