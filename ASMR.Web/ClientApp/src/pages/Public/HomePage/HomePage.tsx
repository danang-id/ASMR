import { useHistory } from "react-router-dom"
import { IoEnterOutline } from "react-icons/io5"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import BaseLayout from "@asmr/layouts/BaseLayout"
import config from "@asmr/libs/common/config"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import "@asmr/pages/Public/HomePage/HomePage.scoped.css"

function HomePage(): JSX.Element {
	useDocumentTitle("Home")
	const history = useHistory()
	const logger = useLogger(HomePage)

	function onTryAsmrButtonClicked() {
		logger.info("Trying application, redirecting to:", DashboardRoutes.IndexPage)
		history.push(DashboardRoutes.IndexPage)
	}

	return (
		<BaseLayout className="page">
			<div className="header">
				<ApplicationLogo/>
				<p className="title">{config.application.name}</p>
			</div>
			<div className="sub-header">
				<p className="sub-title">{config.application.description}</p>
			</div>
			<div className="call-to-action">
				<Button icon={IoEnterOutline} onClick={onTryAsmrButtonClicked}>Try {config.application.name}</Button>
			</div>
		</BaseLayout>
	)
}

export default HomePage
