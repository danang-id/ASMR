import { CSSProperties, ReactNode } from "react"
import { useHistory } from "react-router-dom"
import config from "@asmr/libs/common/config"
import { combineClassNames } from "@asmr/libs/common/styles"
import PublicRoutes from "@asmr/pages/Public/PublicRoutes"
import "@asmr/layouts/BaseLayout/BaseLayout.css"

export interface BaseLayoutProps {
	children?: ReactNode
	className?: string
	ignoreTheme?: boolean
	style?: CSSProperties
}

function BaseLayout({ children, className, ignoreTheme = false, ...props }: BaseLayoutProps): JSX.Element {
	const history = useHistory()
	let addedClassName = combineClassNames("layout")
	if (!ignoreTheme) {
		addedClassName = combineClassNames(addedClassName, "layout-theme")
	}
	className = combineClassNames(addedClassName, className)

	function onApplicationStatusClicked() {
		if (history.location.pathname.startsWith(PublicRoutes.CoreInformationPage)) {
			return
		}

		history.push(PublicRoutes.CoreInformationPage)
	}

	return (
		<div className={className} {...props}>
			{children}
			<div onClick={onApplicationStatusClicked} className="application-status">
				{config.application.name} {config.application.versionFull}
			</div>
		</div>
	)
}

export default BaseLayout
