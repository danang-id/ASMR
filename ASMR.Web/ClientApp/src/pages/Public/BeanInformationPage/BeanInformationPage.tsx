
import { useEffect, useRef, useState } from "react"
import { useHistory } from "react-router-dom"
import { IoPrintOutline } from "react-icons/io5"
import { useReactToPrint } from "react-to-print"
import Button from "@asmr/components/Button"
import QuickResponseCode from "@asmr/components/QuickResponseCode"
import BeanImage from "@asmr/components/BeanImage"
import Bean from "@asmr/data/models/Bean"
import BaseLayout from "@asmr/layouts/BaseLayout"
import config from "@asmr/libs/common/config"
import { getBaseUrl } from "@asmr/libs/common/location"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useProgress from "@asmr/libs/hooks/progressHook"
import useServices from "@asmr/libs/hooks/servicesHook"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"
import "@asmr/pages/Public/BeanInformationPage/BeanInformationPage.scoped.css"

function BeanInformationPage(): JSX.Element {
	useDocumentTitle("Bean Information")
	useInit(onInit)
	const [bean, setBean] = useState<Bean | null>(null)
	const [beanId, setBeanId] = useState("")
	const [initialized, setInitialized] = useState(false)
	const [quickResponseCodeValue, setQuickResponseCodeValue] = useState("")
	const authentication = useAuthentication()
	const history = useHistory()
	const logger = useLogger(BeanInformationPage)
	const notification = useNotification()
	const [progress] = useProgress()
	const services = useServices()

	const printComponentRef = useRef<HTMLDivElement>(null)
	const handlePrintComponent = useReactToPrint({
		content: () => printComponentRef.current
	})

	function onInit() {
		const paths = history.location.pathname.split("/")
		const shouldBeId = paths.pop()
		if (paths.join("/") === PublicRoutes.BeanInformationPage && shouldBeId) {
			setBeanId(shouldBeId)
		}
	}

	function onPrintButtonClicked() {
		if (handlePrintComponent) {
			handlePrintComponent()
		}
	}

	async function retrieveGreenBeanInformation() {
		try {
			const result = await services.bean.getById(beanId)
			if (result.isSuccess && result.data) {
				const url = new URL(PublicRoutes.BeanInformationPage + "/" + result.data.id, getBaseUrl())
				setBean(result.data)
				setQuickResponseCodeValue(url.toString())
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error, notification, logger)
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
								<div className="name">
									Retrieving information...
								</div>
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
					{quickResponseCodeValue && (
						<div className="qr-code">
							<div className="qr-code-image">
								<QuickResponseCode value={quickResponseCodeValue} />
							</div>
						</div>
					)}
					<div className="information">
						<div className="bean">
							<div className="name">
								{ bean ? bean.name : "The bean does not exist"}
							</div>
							<div className="description">{bean?.description}</div>
						</div>
						<div className="about">
							<hr />
							<div>Bean Information</div>
							<div><strong>{config.application.name}</strong></div>
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
							<Button onClick={onPrintButtonClicked} icon={IoPrintOutline}>Print</Button>
						</div>
					)}
				</div>
			)}
		</BaseLayout>
	)
}

export default BeanInformationPage
