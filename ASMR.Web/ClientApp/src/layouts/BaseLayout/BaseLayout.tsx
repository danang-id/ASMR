import { CSSProperties, ReactNode } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import config from "asmr/libs/common/config"
import { combineClassNames } from "asmr/libs/common/styles"
import "asmr/layouts/BaseLayout/BaseLayout.css"

export interface BaseLayoutProps {
	children?: ReactNode
	className?: string
	ignoreTheme?: boolean
	style?: CSSProperties
}

function BaseLayout({ children, className, ignoreTheme = false, ...props }: BaseLayoutProps): JSX.Element {
	const location = useLocation()
	const navigate = useNavigate()
	let addedClassName = combineClassNames("layout")
	if (!ignoreTheme) {
		addedClassName = combineClassNames(addedClassName, "layout-theme")
	}
	className = combineClassNames(addedClassName, className)

	function onApplicationStatusClicked() {
		if (location.pathname.startsWith("/pub/core")) {
			return
		}

		navigate("/pub/core")
	}

	return (
		<div className={className} {...props}>
			{children}
			<div onClick={onApplicationStatusClicked} className="application-status">
				{config.application.name} v{config.application.versionFull}
			</div>
		</div>
	)
}

export default BaseLayout
