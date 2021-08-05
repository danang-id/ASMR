import { ReactNode, useEffect, useState } from "react"
import { useHistory } from "react-router-dom"
import { IoAnalytics, IoBag, IoCash, IoPeople } from "react-icons/io5"
import { GiCoffeeBeans } from "react-icons/gi"
import Role from "@asmr/data/enumerations/Role"
import DashboardLayout from "@asmr/layouts/DashboardLayout"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useDocumentTitle from "@asmr/libs/hooks/documentTitleHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import "@asmr/pages/Dashboard/IndexPage/IndexPage.scoped.css"

type FeaturePage = {
	id: number
	name: string
	icon: ReactNode
	pathname?: string
}

function addFeaturePage(featurePages: FeaturePage[], featurePage: FeaturePage): FeaturePage[] {
	if (!featurePages.find(fp => fp.id === featurePage.id)) {
		featurePages.push(featurePage)
	}

	return featurePages
}

function getFeaturePagesBasedOnRole(featurePages: FeaturePage[], role: Role): FeaturePage[] {
	switch (role) {
		case Role.Administrator:
			featurePages = addFeaturePage(featurePages, {
				id: 0,
				name: "Manage Users",
				icon: <IoPeople />,
				pathname: DashboardRoutes.UsersManagementPage
			})
			featurePages = addFeaturePage(featurePages, {
				id: 1,
				name: "Manage Beans",
				icon: <GiCoffeeBeans />,
				pathname: DashboardRoutes.BeansManagementPage
			})
			featurePages = addFeaturePage(featurePages, {
				id: 2,
				name: "Manage Products",
				icon: <IoBag />,
				pathname: DashboardRoutes.ProductsManagementPage
			})
			featurePages = addFeaturePage(featurePages, {
				id: 3,
				name: "Production Report",
				icon: <IoAnalytics />,
				pathname: DashboardRoutes.ProductionReportPage
			})
			break;
		case Role.Server:
			featurePages = addFeaturePage(featurePages, {
				id: 1,
				name: "Manage Beans",
				icon: <GiCoffeeBeans />,
				pathname: DashboardRoutes.BeansManagementPage
			})
			featurePages = addFeaturePage(featurePages, {
				id: 2,
				name: "Manage Products",
				icon: <IoBag />,
				pathname: DashboardRoutes.ProductsManagementPage
			})
			featurePages = addFeaturePage(featurePages, {
				id: 4,
				name: "Product Transactions",
				icon: <IoCash />,
				pathname: DashboardRoutes.ProductTransactionsPage
			})
			break;
	}

	return featurePages;
}

function IndexPage(): JSX.Element {
	useDocumentTitle("Dashboard")
	const [featurePages, setFeaturePages] = useState<FeaturePage[]>([])
	const authentication = useAuthentication()
	const history = useHistory()
	const notification = useNotification()

	function onFeatureCardClicked(featurePage: FeaturePage) {
		if (!featurePage.pathname) {
			notification.info(`${featurePage.name} feature is not available at the moment.`)
			return
		}

		history.push({ pathname: featurePage.pathname })
	}

	useEffect(() => {
		const roles = authentication.user?.roles.map(userRole => userRole.role) ?? []
		let featurePagesBasedOnRole: FeaturePage[] = []
		for (const role of roles) {
			featurePagesBasedOnRole = getFeaturePagesBasedOnRole(featurePagesBasedOnRole, role)
		}
		featurePagesBasedOnRole = featurePagesBasedOnRole
			.sort((a, b) => a.id < b.id ? -1 : a.id > b.id ? 1 : 0)
		setFeaturePages(featurePagesBasedOnRole)
	}, [authentication.user])

	function renderFeatureCard(featurePage: FeaturePage, index: number) {
		return (
			<div key={index} className="feature-card" onClick={() => onFeatureCardClicked(featurePage)}>
				<div className="feature-icon">{featurePage.icon}</div>
				<p className="feature-name">{featurePage.name}</p>
			</div>
		)
	}

	return (
		<DashboardLayout>
			<div className="feature-card-list">
				{featurePages.map(renderFeatureCard)}
			</div>
		</DashboardLayout>
	)
}

export default IndexPage
