import { useEffect } from "react"
import useLogger from "@asmr/libs/hooks/loggerHook"
import ErrorPage from "@asmr/pages/Errors/ErrorPage"

function OfflinePage(): JSX.Element {
	const logger = useLogger(OfflinePage)

	useEffect(() => {
		logger.info("Offline state detected")
		return () => {
			logger.info("Online state detected")
		}
	}, [])

	return (
		<ErrorPage documentTitle="Offline"
					title="Oops!"
					message="You are seems to be offline. Please make sure you are connected to the Internet to use this application."
		/>
	)
}

export default OfflinePage
