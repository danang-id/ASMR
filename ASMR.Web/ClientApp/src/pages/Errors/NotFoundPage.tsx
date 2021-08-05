import { useEffect } from "react"
import { useHistory, useLocation } from "react-router-dom"
import { IoArrowBack, IoHome } from "react-icons/io5"
import Button from "@asmr/components/Button"
import useLogger from "@asmr/libs/hooks/loggerHook"
import RootRoutes from "@asmr/pages/RootRoutes"
import ErrorPage from "@asmr/pages/Errors/ErrorPage"

function NotFoundPage(): JSX.Element {
	const history = useHistory()
	const location = useLocation()
	const logger = useLogger(NotFoundPage)

	function onBackButtonClicked() {
		if (history) {
			history.goBack()
		} else {
			window.history.back()
		}
	}

	function onHomePageButtonClicked() {
		const homePage = RootRoutes.IndexPage
		if (history) {
			history.push(homePage)
		} else {
			window.location.href = homePage
		}
	}

	useEffect(() => {
		logger.info("Page not found, location:", location.pathname + location.search)
	}, [])

	return (
		<ErrorPage documentTitle="Not Found"
					title="Oops!"
					message="The page you are looking for is not found."
					clickToActions={(
						<>
							<Button icon={IoArrowBack} onClick={onBackButtonClicked}>Back</Button>&nbsp;
							<Button icon={IoHome} onClick={onHomePageButtonClicked}>Home Page</Button>
						</>
					)}
		/>
	)
}

export default NotFoundPage
