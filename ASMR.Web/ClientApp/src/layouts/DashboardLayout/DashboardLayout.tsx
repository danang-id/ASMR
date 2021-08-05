import NavigationHeader from "@asmr/components/NavigationHeader"
import BaseLayout, { BaseLayoutProps } from "@asmr/layouts/BaseLayout/BaseLayout"
import "@asmr/layouts/DashboardLayout/DashboardLayout.css"
import { combineClassNames } from "@asmr/libs/common/styles"

interface DashboardLayoutProps extends BaseLayoutProps {
	contentClassName?: string
}

function DashboardLayout({ children, contentClassName, ...props }: DashboardLayoutProps): JSX.Element {
	contentClassName = combineClassNames("layout-content", contentClassName)

	return (
		<BaseLayout {...props}>
			<NavigationHeader />
			<div className={contentClassName}>{children}</div>
		</BaseLayout>
	)
}

export default DashboardLayout
