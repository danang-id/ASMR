import { ReactNode } from "react"
import BaseLayout from "asmr/layouts/BaseLayout"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import ApplicationLogo from "asmr/components/ApplicationLogo"
import "asmr/pages/Errors/ErrorPage.scoped.css"

export interface ErrorPageProps {
	documentTitle?: string
	title?: string
	message?: string
	clickToActions?: ReactNode
}

function ErrorPage({
	documentTitle = "Error Occurred",
	title = "Something went wrong",
	message = "We are sorry about that. You may try again later.",
	clickToActions,
}: ErrorPageProps): JSX.Element {
	useDocumentTitle(documentTitle)

	return (
		<BaseLayout className="page">
			<div className="header">
				<ApplicationLogo />
			</div>
			<div className="content">
				<p className="title">{title}</p>
				<p className="description">{message}</p>
				<div className="click-to-action">{clickToActions}</div>
			</div>
		</BaseLayout>
	)
}

export default ErrorPage
