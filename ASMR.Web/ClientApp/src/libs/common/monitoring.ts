import ReactGA from "react-ga"
import { getCLS, getFCP, getFID, getLCP, getTTFB, Metric, ReportHandler } from "web-vitals"
import config from "@asmr/libs/common/config"
import Logger from "@asmr/libs/common/logger"

// Learn more: https://bit.ly/CRA-vitals
class Monitoring {
	private static logger = new Logger(Monitoring.name)

	public static sendToGoogleAnalytics({ id, name, value }: Metric) {
		ReactGA.event({
			category: "Web Vitals",
			action: name,
			label: id,
			value: Math.round(name === "CLS" ? value * 1000 : value),
			nonInteraction: true,
		})
	}

	public static reportWebVitals(reportHandler: ReportHandler = Monitoring.logger.info.bind(Monitoring.logger)) {
		if (reportHandler && typeof reportHandler == "function") {
			getCLS(reportHandler)
			getFID(reportHandler)
			getFCP(reportHandler)
			getLCP(reportHandler)
			getTTFB(reportHandler)
		}
	}

	public static reportWebVitalsToGoogleAnalytics() {
		if (config.googleAnalyticsMeasurementID) {
			Monitoring.logger.info(
				"Web Vitals reported to Google Analytics, Measurement ID:",
				config.googleAnalyticsMeasurementID
			)
			Monitoring.reportWebVitals(Monitoring.sendToGoogleAnalytics)
		}
	}
}

export default Monitoring
