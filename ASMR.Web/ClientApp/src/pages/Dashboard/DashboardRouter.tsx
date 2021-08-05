import { Route, Switch } from "react-router-dom"
import ProtectedRoute from "@asmr/components/ProtectedRoute"
import Role from "@asmr/data/enumerations/Role"
import lazy from "@asmr/libs/common/lazy"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"

const IndexPage = lazy(() => import("@asmr/pages/Dashboard/IndexPage"))
const ProfilePage = lazy(() => import("@asmr/pages/Dashboard/ProfilePage"))
const UsersManagementPage = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage"))
const BeansManagementPage = lazy(() => import("@asmr/pages/Dashboard/BeanManagementPage"))
const ProductsManagementPage = lazy(() => import("@asmr/pages/Dashboard/ProductsManagementPage"))
const ProductionReportPage = lazy(() => import("@asmr/pages/Dashboard/ProductionReportPage"))
const ProductTransactionsPage = lazy(() => import("@asmr/pages/Dashboard/ProductTransactionsPage"))
const NotFoundPage = lazy(() => import("@asmr/pages/Errors/NotFoundPage"))
const DashboardRouter = () => (
	<Switch>
		<ProtectedRoute exact path={DashboardRoutes.IndexPage} component={IndexPage} />
		<ProtectedRoute exact path={DashboardRoutes.ProfilePage} component={ProfilePage} />
		<ProtectedRoute exact path={DashboardRoutes.UsersManagementPage} component={UsersManagementPage}
						allowedRoles={[Role.Administrator]} />
		<ProtectedRoute exact path={DashboardRoutes.BeansManagementPage} component={BeansManagementPage}
						allowedRoles={[Role.Administrator, Role.Server]} />
		<ProtectedRoute exact path={DashboardRoutes.ProductsManagementPage} component={ProductsManagementPage}
						allowedRoles={[Role.Administrator, Role.Server]} />
		<ProtectedRoute exact path={DashboardRoutes.ProductionReportPage} component={ProductionReportPage}
						allowedRoles={[Role.Administrator]} />
		<ProtectedRoute exact path={DashboardRoutes.ProductTransactionsPage} component={ProductTransactionsPage}
						allowedRoles={[Role.Server]} />
		<Route component={NotFoundPage}/>
	</Switch>
)

export default DashboardRouter
