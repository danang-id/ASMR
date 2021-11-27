import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { IoEnterOutline, IoLogoAndroid, IoLogoApple } from "react-icons/io5"
import ApplicationLogo from "asmr/components/ApplicationLogo"
import AppStoreButton from "asmr/components/AppStoreButton"
import Button from "asmr/components/Button"
import GooglePlayButton from "asmr/components/GooglePlayButton"
import AndroidReleaseInformation from "asmr/core/release/common/AndroidReleaseInformation"
import iOSReleaseInformation from "asmr/core/release/common/iOSReleaseInformation"
import BaseLayout from "asmr/layouts/BaseLayout"
import config from "asmr/libs/common/config"
import useDocumentTitle from "asmr/libs/hooks/documentTitleHook"
import useInit from "asmr/libs/hooks/initHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useServices from "asmr/libs/hooks/servicesHook"
import AboutModal from "asmr/pages/Public/HomePage/AboutModal"
import "asmr/pages/Public/HomePage/HomePage.scoped.css"

function HomePage(): JSX.Element {
	useDocumentTitle("Home")
	useInit(onInit)
	const [aboutModalShown, setAboutModalShown] = useState(false)
	const [androidPlayStoreLink, setAndroidPlayStoreLink] = useState<string | undefined>()
	const [androidDirectDownloadLink, setAndroidDirectDownloadLink] = useState<string | undefined>()
	const [iosAppStoreLink, setIosAppStoreLink] = useState<string | undefined>()
	const [iosDirectDownloadLink, setIosDirectDownloadLink] = useState<string | undefined>()
	const [hasDirectDownload, setHasDirectDownload] = useState(false)
	const [hasExternalStore, setHasExternalStore] = useState(false)
	const logger = useLogger(HomePage)
	const navigate = useNavigate()
	const services = useServices()

	async function onInit() {
		try {
			const result = await services.release.getMobileReleaseInformation()
			if (result.isSuccess && result.data) {
				setAndroidReleaseInformation(result.data.Android)
				setIosReleaseInformation(result.data.iOS)
			}

			services.handleErrors(result.errors, void 0, logger)
		} catch (error) {
			services.handleError(error as Error, void 0, logger)
		}
	}

	function onCloseModals() {
		setAboutModalShown(false)
	}

	function onShowAboutModalButtonClicked() {
		setAboutModalShown(true)
	}

	function onOpenLink(url: string) {
		return () => window.open(url, "_blank")
	}

	function onSignInButtonClicked() {
		logger.info("Trying application, redirecting to:", "/dashboard")
		navigate("/dashboard")
	}

	function setAndroidReleaseInformation(releaseInformation: AndroidReleaseInformation) {
		if (!releaseInformation) {
			setAndroidPlayStoreLink(void 0)
			setAndroidDirectDownloadLink(void 0)
		}

		if (
			releaseInformation.PlayStore &&
			releaseInformation.PlayStore.Available &&
			releaseInformation.PlayStore.Link
		) {
			setAndroidPlayStoreLink(releaseInformation.PlayStore.Link)
			setHasExternalStore(true)
		} else {
			setAndroidPlayStoreLink(void 0)
		}

		if (
			releaseInformation.DirectDownload &&
			releaseInformation.DirectDownload.Available &&
			releaseInformation.DirectDownload.Link
		) {
			setAndroidDirectDownloadLink(releaseInformation.DirectDownload.Link)
			setHasDirectDownload(true)
		} else {
			setAndroidDirectDownloadLink(void 0)
		}
	}

	function setIosReleaseInformation(releaseInformation: iOSReleaseInformation) {
		if (!releaseInformation) {
			setIosAppStoreLink(void 0)
			setIosDirectDownloadLink(void 0)
		}

		if (releaseInformation.AppStore && releaseInformation.AppStore.Available && releaseInformation.AppStore.Link) {
			setIosAppStoreLink(releaseInformation.AppStore.Link)
			setHasExternalStore(true)
		} else {
			setIosAppStoreLink(void 0)
		}

		if (
			releaseInformation.DirectDownload &&
			releaseInformation.DirectDownload.Available &&
			releaseInformation.DirectDownload.Link
		) {
			setIosDirectDownloadLink(releaseInformation.DirectDownload.Link)
			setHasDirectDownload(true)
		} else {
			setIosDirectDownloadLink(void 0)
		}
	}

	return (
		<BaseLayout className="page">
			<div className="header">
				<ApplicationLogo />
				<p className="title">{config.application.name}</p>
			</div>
			<div className="description">
				<Button style="none" onClick={onShowAboutModalButtonClicked}>
					What is {config.application.name}?
				</Button>
			</div>
			<span className="separator" />
			<div className="call-to-action">
				<Button onClick={onSignInButtonClicked}>
					Sign In&nbsp;&nbsp;
					<IoEnterOutline />
				</Button>
				{hasDirectDownload && (
					<div className="direct-download">
						{androidDirectDownloadLink && (
							<Button style="outline" onClick={onOpenLink(androidDirectDownloadLink)}>
								<IoLogoAndroid />&nbsp;&nbsp;Download for Android
							</Button>
						)}
						{iosDirectDownloadLink && (
							<Button style="outline" onClick={onOpenLink(iosDirectDownloadLink)}>
								<IoLogoApple />&nbsp;&nbsp;Download for iOS
							</Button>
						)}
					</div>
				)}
				{hasExternalStore && (
					<div className="external-store">
						{androidPlayStoreLink && <GooglePlayButton link={androidPlayStoreLink} />}
						{iosAppStoreLink && <AppStoreButton link={iosAppStoreLink} />}
					</div>
				)}
			</div>

			<AboutModal onClose={onCloseModals} onTryAsmr={onSignInButtonClicked} show={aboutModalShown} />
		</BaseLayout>
	)
}

export default HomePage
