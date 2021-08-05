import ReactGA from "react-ga"
import { Route, Switch, useHistory } from "react-router-dom"
import { Location } from "history"
import ProtectedRoute from "@asmr/components/ProtectedRoute"
import lazy from "@asmr/libs/common/lazy"
import { getBaseUrl } from "@asmr/libs/common/location"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNetwork from "@asmr/libs/hooks/networkHook"
import OfflinePage from "@asmr/pages/Errors/OfflinePage"
import RootRoutes from "@asmr/pages/RootRoutes"

const PublicRouter = lazy(() => import("@asmr/pages/Public/PublicRouter"))
const AuthenticationRouter = lazy(() => import("@asmr/pages/Authentication/AuthenticationRouter"))
const DashboardRouter = lazy(() => import("@asmr/pages/Dashboard/DashboardRouter"))
const NotFoundPage = lazy(() => import("@asmr/pages/Errors/NotFoundPage"))
function RootRouter(): JSX.Element {
	useInit(onInit)
	const history = useHistory()
	const logger = useLogger(RootRouter)
	const network = useNetwork()

	function onInit() {
		history.listen(location => {
			const pathname = logLocation(location)
			ReactGA.set({ page: pathname })
			ReactGA.pageview((location as any).pathname);
		})
		logLocation(history.location)
	}

	function isEmptyIterable(iterableIterator: IterableIterator<any>) {
		const next = iterableIterator.next()
		return typeof next.value === "undefined" && next.done
	}

	function logLocation(location: Location) {
		const search = new URLSearchParams(location.search)
		const pathname = isEmptyIterable(search.values())
			? location.pathname
			: location.pathname + "?" + search.toString()
		const url = new URL(pathname, getBaseUrl())
		logger.info("Location pushed:", url.toString())
		return pathname
	}

	if (!network.onLine) {
		return <OfflinePage />
	}

	return (
		<Switch>
			<Route exact path={RootRoutes.IndexPage} component={PublicRouter} />
			<Route path={RootRoutes.PublicRouter} component={PublicRouter} />
			<Route path={RootRoutes.AuthenticationRouter} component={AuthenticationRouter} />
			<ProtectedRoute path={RootRoutes.DashboardRouter} component={DashboardRouter} />
			<Route component={NotFoundPage}/>
		</Switch>
	)
}

export default RootRouter
