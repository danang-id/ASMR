import { useEffect, useState } from "react"
import ASMRWebReleaseInformation from "asmr/core/release/ASMRWebReleaseInformation"
import BackButton from "asmr/components/BackButton"
import Table from "asmr/components/Table"
import BaseLayout from "asmr/layouts/BaseLayout"
import config from "asmr/libs/common/config"
import environment from "asmr/libs/common/environment"
import useInit from "asmr/libs/hooks/initHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useNotification from "asmr/libs/hooks/notificationHook"
import useServices from "asmr/libs/hooks/servicesHook"
import "asmr/pages/Public/CoreInformationPage/CoreInformationPage.scoped.css"

function CoreInformationPage(): JSX.Element {
	useInit(onInit)
	const [releaseInformation, setReleaseInformation] = useState<ASMRWebReleaseInformation | null>(null)
	const [isBackEndLatest, setIsBackEndLatest] = useState(true)
	const [isFrontEndLatest, setIsFrontEndLatest] = useState(true)
	const logger = useLogger(CoreInformationPage)
	const notification = useNotification()
	const services = useServices()

	async function onInit() {
		await retrieveWebReleaseInformation()
	}

	async function retrieveWebReleaseInformation() {
		try {
			const result = await services.release.getWebReleaseInformation()
			if (result.isSuccess && result.data) {
				setReleaseInformation(result.data)
			}

			services.handleErrors(result.errors, notification, logger)
		} catch (error) {
			services.handleError(error as Error, notification, logger)
		}
	}

	useEffect(() => {
		if (!releaseInformation) {
			return
		}

		setIsBackEndLatest(releaseInformation.BackEnd.Version === config.backEndVersion)
		setIsFrontEndLatest(releaseInformation.FrontEnd.Version == config.application.versionFull)
	}, [releaseInformation])

	return (
		<BaseLayout>
			<div className="core-information-page">
				<div className="header">
					<BackButton />
					&nbsp;&nbsp; Core Information
				</div>
				<div className="table-container">
					<Table>
						<Table.Head>
							<Table.Row>
								<Table.DataCell head>System</Table.DataCell>
								<Table.DataCell head>Version</Table.DataCell>
							</Table.Row>
						</Table.Head>
						<Table.Body>
							<Table.Row>
								<Table.DataCell>APIs</Table.DataCell>
								<Table.DataCell>
									<p className="latest-version">
										Latest Version: {releaseInformation?.BackEnd.Version ?? "N/A"}
									</p>
									{isBackEndLatest ? (
										<p className="current-version">You are using the latest version</p>
									) : (
										<p className="current-version">
											Version mismatch, you are using version {config.backEndVersion}
											<br />
											You may try to refresh or clear the cache of the web application
										</p>
									)}
								</Table.DataCell>
							</Table.Row>
							<Table.Row>
								<Table.DataCell>Web UI</Table.DataCell>
								<Table.DataCell>
									{environment.isDevelopment && (
										<>
											<p className="current-version">
												You are on the development version of the Web UI. This information may
												not be relevant for you.
											</p>
											<hr />
										</>
									)}
									<p className="latest-version">
										Latest Version: {releaseInformation?.FrontEnd.Version ?? "N/A"}
									</p>
									{isFrontEndLatest ? (
										<p className="current-version">You are using the latest version</p>
									) : (
										<p className="current-version">
											Version mismatch, you are using version {config.application.versionFull}
											<br />
											You may try to refresh or clear the cache of the web application
										</p>
									)}
								</Table.DataCell>
							</Table.Row>
						</Table.Body>
					</Table>
				</div>
			</div>
		</BaseLayout>
	)
}

export default CoreInformationPage
