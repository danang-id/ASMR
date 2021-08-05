import * as React from "react"
import { CSSProperties, ReactNode } from "react"
import config from "@asmr/libs/common/config"
import { combineClassNames } from "@asmr/libs/common/styles"
import "@asmr/layouts/BaseLayout/BaseLayout.css"
import environment from "@asmr/libs/common/environment"

export interface BaseLayoutProps {
	children?: ReactNode
	className?: string
	ignoreTheme?: boolean
	style?: CSSProperties
}

function BaseLayout({ children, className, ignoreTheme = false, ...props }: BaseLayoutProps): JSX.Element {
	let addedClassName = combineClassNames("layout")
	if (!ignoreTheme) {
		addedClassName = combineClassNames(addedClassName, "layout-theme")
	}
	className = combineClassNames(addedClassName, className)

	return (
		<div className={className} {...props}>
			{children}
			<div className="application-status">
				{config.application.name} {config.application.version}.{config.build.number}{" "}
				{environment.isDevelopment && "(Development Environment)"}
				{environment.isTest && "(Test Environment)"}
			</div>
		</div>
	)
}

export default BaseLayout
