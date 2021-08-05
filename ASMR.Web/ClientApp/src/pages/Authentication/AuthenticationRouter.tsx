import { Route, Switch } from "react-router-dom"
import lazy from "@asmr/libs/common/lazy"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"

const SignInPage = lazy(() => import("@asmr/pages/Authentication/SignInPage"))
const SignOutPage = lazy(() => import("@asmr/pages/Authentication/SignOutPage"))
const NotFoundPage = lazy(() => import("@asmr/pages/Errors/NotFoundPage"))
const AuthenticationRouter = () => (
	<Switch>
		<Route exact path={AuthenticationRoutes.SignInPage} component={SignInPage}  />
		<Route exact path={AuthenticationRoutes.SignOutPage} component={SignOutPage} />
		<Route component={NotFoundPage}/>
	</Switch>
)

export default AuthenticationRouter
