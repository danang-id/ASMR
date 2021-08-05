import { useEffect, useState } from "react"
import { useHistory, useLocation } from "react-router-dom"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import UserImage from "@asmr/components/UserImage"
import config from "@asmr/libs/common/config"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import AuthenticationRoutes from "@asmr/pages/Authentication/AuthenticationRoutes"
import DashboardRoutes from "@asmr/pages/Dashboard/DashboardRoutes"
import "@asmr/components/styles/NavigationHeader.css"

function NavigationHeader(): JSX.Element {
	const [userFullName, setUserFullName] = useState("")
	const [profileMenuShown, setProfileMenuShown] = useState(false)
	const [hoverTimeout, setHoverTimeout] = useState<NodeJS.Timeout | null>(null)
	const authentication = useAuthentication()
	const history = useHistory()
	const location = useLocation()

	function onApplicationLogoClicked() {
		history.push(DashboardRoutes.IndexPage)
	}

	function onProfileMenuClicked() {
		setProfileMenuShown(!profileMenuShown)
	}

	function onProfileMenuOverlayMouseEntered() {
		setProfileMenuShown(true)
	}

	function onProfileMenuOverlayMouseLeft() {
		const timeout = setTimeout(() => {
			setProfileMenuShown(false)
		}, 200)
		setHoverTimeout(timeout)
	}

	function onProfileClicked() {
		if (location.pathname.startsWith(DashboardRoutes.ProfilePage)) {
			return
		}

		history.push(DashboardRoutes.ProfilePage)
	}

	function onSignOutClicked() {
		history.push(AuthenticationRoutes.SignOutPage)
	}

	useEffect(() => {
		if (!authentication.user) {
			setUserFullName("")
			return
		}
		const userLastName = authentication.user.lastName
			.split(" ")
			.map(word => word.substr(0, 1))
			.join(" ")
		setUserFullName(`${authentication.user.firstName} ${userLastName}`)
	}, [authentication.user])

	useEffect(() => {
		return () => {
			if (hoverTimeout) {
				clearTimeout(hoverTimeout)
			}
		}
	}, [hoverTimeout])

	return (
		<div className="navigation-header">
			<div className="navigation-bar">
				<div className="navigation-bar-left" onClick={onApplicationLogoClicked}>
					<ApplicationLogo />
					<p className="application-name">{config.application.name}</p>
				</div>
				{
					authentication.user && (
						<div className="navigation-bar-right">
							<div className="profile-menu" onClick={onProfileMenuClicked}>
								<div className="profile-image">
									<UserImage circular user={authentication.user} />
								</div>
								<div className="profile-information">
									<p className="profile-name ">
										{userFullName}
									</p>
								</div>
							</div>
						</div>
					)
				}
			</div>
			{
				(authentication.user && profileMenuShown) && (
					<div className="profile-menu-overlay"
						onMouseEnter={onProfileMenuOverlayMouseEntered}
						onMouseLeave={onProfileMenuOverlayMouseLeft}>
						<Button className="profile-menu-item" inverted onClick={onProfileClicked}>My Profile</Button>
						<div className="profile-menu-separator" />
						<Button className="profile-menu-item" inverted onClick={onSignOutClicked}>Sign Out</Button>
					</div>
				)
			}
		</div>
	)
}

export default NavigationHeader
