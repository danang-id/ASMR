import { Route, Switch } from "react-router-dom"
import lazy from "@asmr/libs/common/lazy"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"

const HomePage = lazy(() => import("@asmr/pages/Public/HomePage"))
const BeanInformationPage = lazy(() => import("@asmr/pages/Public/BeanInformationPage"))
const ProductInformationPage = lazy(() => import("@asmr/pages/Public/ProductInformationPage"))
const NotFoundPage = lazy(() => import("@asmr/pages/Errors/NotFoundPage"))
function PublicRouter(): JSX.Element {
	return (
		<Switch>
			<Route exact path={PublicRoutes.HomePage} component={HomePage} />
			<Route path={PublicRoutes.BeanInformationPage} component={BeanInformationPage} />
			<Route path={PublicRoutes.ProductInformationPage} component={ProductInformationPage} />
			<Route component={NotFoundPage}/>
		</Switch>
	)
}

export default PublicRouter
