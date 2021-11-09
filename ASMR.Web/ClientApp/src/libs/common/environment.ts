import config from "@asmr/libs/common/config"

const environment = {
	isDevelopment: config.nodeEnv === "development",
	isProduction: config.nodeEnv === "production",
	isTest: config.nodeEnv === "test",
}

export default environment
