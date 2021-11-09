import { StrictMode } from "react"
import ReactDOM from "react-dom"
import { ErrorBoundary } from "react-error-boundary"
import ReactGA from "react-ga"

import Application from "@asmr/Application"
import ServiceWorker from "@asmr/serviceWorkerRegistration"
import config from "@asmr/libs/common/config"
import banner from "@asmr/libs/common/banner"
import Monitoring from "@asmr/libs/common/monitoring"
import FallbackPage from "@asmr/pages/Errors/FallbackPage"
import "@asmr/styles/global.css"

banner.show()

if (config.googleAnalyticsMeasurementID) {
	ReactGA.initialize(config.googleAnalyticsMeasurementID)

	Monitoring.reportWebVitalsToGoogleAnalytics()
}

ReactDOM.render(
	<StrictMode>
		<ErrorBoundary FallbackComponent={FallbackPage}>
			<Application />
		</ErrorBoundary>
	</StrictMode>,
	document.getElementById("asmr")
)

if (process.env.NODE_ENV === "production") {
	ServiceWorker.register()
}
