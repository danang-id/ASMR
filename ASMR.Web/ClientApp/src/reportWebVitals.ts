import { getCLS, getFID, getFCP, getLCP, getTTFB, ReportHandler } from "web-vitals"
import Logger from "asmr/libs/common/logger"

const logger = new Logger("WebVital")
function reportWebVitals(reportHandler: ReportHandler = logger.info.bind(logger)) {
	if (reportHandler && typeof reportHandler == "function") {
		getCLS(reportHandler)
		getFID(reportHandler)
		getFCP(reportHandler)
		getLCP(reportHandler)
		getTTFB(reportHandler)
	}
}

export default reportWebVitals
