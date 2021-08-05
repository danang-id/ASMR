import build from "@asmr/build.json"

const now = new Date()
const monthDigitList = "ABCDEFGHIJKL"
const dateDigitList = "abcdefghijklmnopqrstuvwxyzABCDE"
const yearDigit = now.getFullYear().toString().substring(2)
const monthDigit = monthDigitList.charAt(now.getMonth())
const dateDigit = dateDigitList.charAt(now.getDate() - 1)
const timeDigit = now.getHours() >= 10 ? now.getHours().toString() : `0${now.getHours()}`

const buildNumber = process.env.NODE_ENV === "production"
	? (build.number ?? "00000000")
	:  yearDigit + monthDigit + dateDigit + timeDigit

const config = {
	application: {
		name: process.env.REACT_APP_NAME ?? "ASMR",
		version: process.env.REACT_APP_VERSION ?? "1.0",
		description: process.env.REACT_APP_DESCRIPTION ?? "Coffee Beans Management Solution"
	},
	build: {
		number: buildNumber
	},
	nodeEnv: process.env.NODE_ENV as "production" | "development" | "test",
	publicUrl: process.env.PUBLIC_URL,
	googleAnalyticsMeasurementID: process.env.REACT_APP_GOOGLE_ANALYTICS_MEASUREMENT_ID,
	sentryDSN: process.env.REACT_APP_SENTRY_DSN,
}

export default config
