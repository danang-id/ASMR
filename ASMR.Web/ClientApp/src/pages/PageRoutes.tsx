import ProtectedPage from "@asmr/components/ProtectedPage"

declare global {
	interface Window {
		serviceWorkerUpdateReady?: boolean
	}
}

import { useEffect } from "react"
import ReactGA from "react-ga"
import { Route, Routes, useLocation } from "react-router-dom"
import { Location } from "history"
import lazy from "@asmr/libs/common/lazy"
import { getBaseUrl } from "@asmr/libs/common/location"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNetwork from "@asmr/libs/hooks/networkHook"
import OfflinePage from "@asmr/pages/Errors/OfflinePage"
import Role from "@asmr/data/enumerations/Role"

// Public Pages
const HomePage = lazy(() => import("@asmr/pages/Public/HomePage"))
const CoreInformationPage = lazy(() => import("@asmr/pages/Public/CoreInformationPage"))
const BeanInformationPage = lazy(() => import("@asmr/pages/Public/BeanInformationPage"))
const ProductInformationPage = lazy(() => import("@asmr/pages/Public/ProductInformationPage"))

// Authentication Pages
const RegistrationPage = lazy(() => import("@asmr/pages/Authentication/RegistrationPage"))
const ConfirmEmailAddressPage = lazy(() => import("@asmr/pages/Authentication/ConfirmEmailAddressPage"))
const SignInPage = lazy(() => import("@asmr/pages/Authentication/SignInPage"))
const SignOutPage = lazy(() => import("@asmr/pages/Authentication/SignOutPage"))
const ForgetPasswordPage = lazy(() => import("@asmr/pages/Authentication/ForgetPasswordPage"))
const ResetPasswordPage = lazy(() => import("@asmr/pages/Authentication/ResetPasswordPage"))

// Dashboard Pages
const IndexPage = lazy(() => import("@asmr/pages/Dashboard/IndexPage"))
const ProfilePage = lazy(() => import("@asmr/pages/Dashboard/ProfilePage"))
const UsersManagementPage = lazy(() => import("@asmr/pages/Dashboard/UsersManagementPage"))
const BeansManagementPage = lazy(() => import("@asmr/pages/Dashboard/BeanManagementPage"))
const ProductsManagementPage = lazy(() => import("@asmr/pages/Dashboard/ProductsManagementPage"))
const ProductionReportPage = lazy(() => import("@asmr/pages/Dashboard/ProductionReportPage"))
const ProductTransactionsPage = lazy(() => import("@asmr/pages/Dashboard/ProductTransactionsPage"))

// Misc Pages
const NotFoundPage = lazy(() => import("@asmr/pages/Errors/NotFoundPage"))

function PageRoutes(): JSX.Element {
	useInit(onInit)
	const location = useLocation()
	const logger = useLogger(PageRoutes)
	const network = useNetwork()

	function onLocationChanged() {
		const pathname = logLocation(location)
		ReactGA.set({ page: pathname })
		ReactGA.pageview((location as any).pathname)

		if (window.serviceWorkerUpdateReady) {
			window.serviceWorkerUpdateReady = false
			window.stop()
			window.location.reload()
		}
	}

	function onInit() {
		onLocationChanged()
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

	useEffect(() => {
		onLocationChanged()
	}, [location.pathname])

	if (!network.onLine) {
		return <OfflinePage />
	}

	return (
		<Routes>
			<Route index element={<HomePage />} />
			<Route path="pub">
				<Route index element={<HomePage />} />
				<Route path="core" element={<CoreInformationPage />} />
				<Route path="bean/:beanId" element={<BeanInformationPage />} />
				<Route path="product/:productId" element={<ProductInformationPage />} />
			</Route>
			<Route path="authentication">
				<Route path="register" element={<RegistrationPage />} />
				<Route path="sign-in" element={<SignInPage />} />
				<Route path="sign-out" element={<SignOutPage />} />
				<Route path="email-address">
					<Route path="confirm" element={<ConfirmEmailAddressPage />} />
				</Route>
				<Route path="password">
					<Route path="forget" element={<ForgetPasswordPage />} />
					<Route path="reset" element={<ResetPasswordPage />} />
				</Route>
			</Route>

			<Route path="dashboard">
				<Route
					index
					element={
						<ProtectedPage>
							<IndexPage />
						</ProtectedPage>
					}
				/>
				<Route
					path="profile"
					element={
						<ProtectedPage>
							<ProfilePage />
						</ProtectedPage>
					}
				/>
				<Route path="manage">
					<Route
						path="users"
						element={
							<ProtectedPage allowedRoles={[Role.Administrator]}>
								<UsersManagementPage />
							</ProtectedPage>
						}
					/>
					<Route
						path="beans"
						element={
							<ProtectedPage allowedRoles={[Role.Administrator, Role.Server]}>
								<BeansManagementPage />
							</ProtectedPage>
						}
					/>
					<Route
						path="products"
						element={
							<ProtectedPage allowedRoles={[Role.Administrator, Role.Server]}>
								<ProductsManagementPage />
							</ProtectedPage>
						}
					/>
				</Route>
				<Route path="report">
					<Route
						path="production"
						element={
							<ProtectedPage allowedRoles={[Role.Administrator]}>
								<ProductionReportPage />
							</ProtectedPage>
						}
					/>
				</Route>
				<Route
					path="transactions"
					element={
						<ProtectedPage allowedRoles={[Role.Server]}>
							<ProductTransactionsPage />
						</ProtectedPage>
					}
				/>
			</Route>

			<Route path="*" element={<NotFoundPage />} />
		</Routes>
	)
}

export default PageRoutes
