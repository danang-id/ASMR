import { useEffect, useState } from "react"
import { useHistory } from "react-router-dom"
import { FallbackProps } from "react-error-boundary"
import { IoArrowBack, IoHome } from "react-icons/io5"
import Button from "@asmr/components/Button"
import environment from "@asmr/libs/common/environment"
import useLogger from "@asmr/libs/hooks/loggerHook"
import ErrorPage from "@asmr/pages/Errors/ErrorPage"
import RootRoutes from "@asmr/pages/RootRoutes"

interface FallbackPageProps extends FallbackProps {
	error: Error & { supportId?: string }
}

function FallbackPage({ error, resetErrorBoundary }: FallbackPageProps): JSX.Element {
	const [showMessage, setShowMessage] = useState<boolean>(false)
	const history = useHistory()
	const logger = useLogger(FallbackPage)


	function onBackButtonClicked() {
		if (history) {
			history.goBack()
		} else {
			window.history.back()
		}
		resetErrorBoundary()
	}

	function onHomePageButtonClicked() {
		const homePage = RootRoutes.IndexPage
		if (history) {
			history.push(homePage)
		} else {
			window.location.href = homePage
		}
		resetErrorBoundary()
	}

	useEffect(() => {
		const hasErrorMessage = typeof error?.message !== "undefined" && error.message.length > 0
		setShowMessage(environment.isDevelopment && hasErrorMessage)
		if (hasErrorMessage) {
			logger.info(error)
		}
	}, [error])

	return (
		<ErrorPage documentTitle="Error Occurred"
					message={showMessage ? error?.message : void 0}
					clickToActions={(
						<>
							<Button icon={IoArrowBack} onClick={onBackButtonClicked}>Back</Button>&nbsp;
							<Button icon={IoHome} onClick={onHomePageButtonClicked}>Home Page</Button>
						</>
					)}
		/>
	)
}

export default FallbackPage
