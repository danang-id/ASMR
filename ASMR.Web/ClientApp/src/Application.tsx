import { Suspense } from "react"
import { BrowserRouter } from "react-router-dom"
import LoadingBar from "asmr/components/LoadingBar"
import ApplicationProvider from "asmr/libs/application/ApplicationProvider"
import lazy from "asmr/libs/common/lazy"
import PageRoutes from "asmr/pages/PageRoutes"
import SuspenseFallbackPage from "asmr/pages/Misc/SuspenseFallbackPage"

const DeveloperMenu =
	process.env.NODE_ENV === "development"
		? lazy(() => import("asmr/components/DeveloperMenu"))
        : lazy(() => import("asmr/components/NoDeveloperMenu"))

const Application = () => (
	<ApplicationProvider>
		<BrowserRouter>
			<LoadingBar showSpinner />
			<Suspense fallback={<SuspenseFallbackPage />}>
				<DeveloperMenu />
				<PageRoutes />
			</Suspense>
		</BrowserRouter>
	</ApplicationProvider>
)

export default Application
