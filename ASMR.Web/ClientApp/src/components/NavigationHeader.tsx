import { Fragment, useEffect, useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import { Menu, Transition } from "@headlessui/react"
import ApplicationLogo from "@asmr/components/ApplicationLogo"
import Button from "@asmr/components/Button"
import UserImage from "@asmr/components/UserImage"
import config from "@asmr/libs/common/config"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import "@asmr/components/styles/NavigationHeader.css"

function NavigationHeader(): JSX.Element {
	const [userFullName, setUserFullName] = useState("")
	// const [profileMenuShown, setProfileMenuShown] = useState(false)
	// const [hoverTimeout, setHoverTimeout] = useState<NodeJS.Timeout | null>	(null)
	const authentication = useAuthentication()
	const location = useLocation()
	const navigate = useNavigate()

	function onApplicationLogoClicked() {
		navigate("/dashboard")
	}

	// function onProfileMenuClicked() {
	// 	setProfileMenuShown(!profileMenuShown)
	// }

	// function onProfileMenuOverlayMouseEntered() {
	// 	setProfileMenuShown(true)
	// }
	//
	// function onProfileMenuOverlayMouseLeft() {
	// 	const timeout = setTimeout(() => {
	// 		setProfileMenuShown(false)
	// 	}, 200)
	// 	setHoverTimeout(timeout)
	// }

	function onProfileClicked() {
		if (location.pathname.startsWith("/dashboard/profile")) {
			return
		}

		navigate("/dashboard/profile")
	}

	function onSignOutClicked() {
		navigate("/authentication/sign-out")
	}

	useEffect(() => {
		if (!authentication.user) {
			setUserFullName("")
			return
		}
		const userLastName = authentication.user.lastName
			.split(" ")
			.map((word) => word.substr(0, 1))
			.join(" ")
		setUserFullName(`${authentication.user.firstName} ${userLastName}`)
	}, [authentication.user])

	// useEffect(() => {
	// 	return () => {
	// 		if (hoverTimeout) {
	// 			clearTimeout(hoverTimeout)
	// 		}
	// 	}
	// }, [hoverTimeout])

	return (
		<div className="navigation-header">
			<div className="navigation-bar">
				<div className="navigation-bar-left" onClick={onApplicationLogoClicked}>
					<ApplicationLogo />
					<p className="application-name">{config.application.name}</p>
				</div>
				{authentication.user && (
					<Menu as="div" className="navigation-bar-right">
						<Menu.Button className="profile-menu">
							<div className="profile-image">
								<UserImage circular user={authentication.user} />
							</div>
							<div className="profile-information">
								<p className="profile-name ">{userFullName}</p>
							</div>
						</Menu.Button>
						<Transition
							as={Fragment}
							enter="transition ease-out duration-100"
							enterFrom="transform opacity-0 scale-95"
							enterTo="transform opacity-100 scale-100"
							leave="transition ease-in duration-75"
							leaveFrom="transform opacity-100 scale-100"
							leaveTo="transform opacity-0 scale-95"
						>
							<Menu.Items className="profile-menu-overlay">
								<Menu.Item>
									<Button className="profile-menu-item" style="outline" onClick={onProfileClicked}>
										My Profile
									</Button>
								</Menu.Item>
								<Menu.Item>
									<Button className="profile-menu-item" style="outline" onClick={onSignOutClicked}>
										Sign Out
									</Button>
								</Menu.Item>
							</Menu.Items>
						</Transition>
					</Menu>
				)}
			</div>
		</div>
	)
}

export default NavigationHeader
