import { Suspense } from "react"
import { BrowserRouter } from "react-router-dom"
import LoadingBar from "@asmr/components/LoadingBar"
import ApplicationProvider  from "@asmr/libs/application/ApplicationProvider"
import lazy from "@asmr/libs/common/lazy"
import SuspenseFallbackPage from "@asmr/pages/Misc/SuspenseFallbackPage"

const RootRouter = lazy(() => import("@asmr/pages/RootRouter"))
const DeveloperMenu = process.env.NODE_ENV === "development"
	? lazy(() => import("@asmr/components/DeveloperMenu"))
	: lazy(() => import("@asmr/components/NoDeveloperMenu"))

const Application = () => (
	<ApplicationProvider>
		<LoadingBar showSpinner />
		<Suspense fallback={<SuspenseFallbackPage />}>
			<BrowserRouter>
				<DeveloperMenu />
				<RootRouter />
			</BrowserRouter>
		</Suspense>
	</ApplicationProvider>
)

export default Application
