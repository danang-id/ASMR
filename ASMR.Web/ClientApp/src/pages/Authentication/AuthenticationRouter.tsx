import { Route, Switch } from "react-router-dom"
import lazy from "@asmr/libs/common/lazy"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"

const RegistrationPage = lazy(() => import("@asmr/pages/Authentication/RegistrationPage"))
const ConfirmEmailAddressPage = lazy(() => import("@asmr/pages/Authentication/ConfirmEmailAddressPage"))
const SignInPage = lazy(() => import("@asmr/pages/Authentication/SignInPage"))
const SignOutPage = lazy(() => import("@asmr/pages/Authentication/SignOutPage"))
const ForgetPasswordPage = lazy(() => import("@asmr/pages/Authentication/ForgetPasswordPage"))
const ResetPasswordPage = lazy(() => import("@asmr/pages/Authentication/ResetPasswordPage"))
const NotFoundPage = lazy(() => import("@asmr/pages/Errors/NotFoundPage"))
const AuthenticationRouter = () => (
	<Switch>
		<Route exact path={AuthenticationRoutes.RegistrationPage} component={RegistrationPage}  />
		<Route path={AuthenticationRoutes.ConfirmEmailAddressPage} component={ConfirmEmailAddressPage}  />
		<Route exact path={AuthenticationRoutes.SignInPage} component={SignInPage}  />
		<Route exact path={AuthenticationRoutes.SignOutPage} component={SignOutPage} />
		<Route exact path={AuthenticationRoutes.ForgetPasswordPage} component={ForgetPasswordPage}  />
		<Route path={AuthenticationRoutes.ResetPasswordPage} component={ResetPasswordPage} />
		<Route component={NotFoundPage}/>
	</Switch>
)

export default AuthenticationRouter
