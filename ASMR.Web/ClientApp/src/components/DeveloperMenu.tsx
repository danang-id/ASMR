import { useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { useErrorHandler } from "react-error-boundary"
import {
	IoBug,
	IoCog,
	IoKeyOutline,
	IoLogInOutline,
	IoLogOutOutline,
	IoMoon,
	IoMoonOutline,
	IoPersonOutline,
	IoWarning,
} from "react-icons/io5"
import { Action, Fab } from "react-tiny-fab"
import copy from "copy-to-clipboard"
import { base64ToHex, hexToASCII } from "@asmr/libs/common/encoding"
import environment from "@asmr/libs/common/environment"
import useAuthentication from "@asmr/libs/hooks/authenticationHook"
import useLogger from "@asmr/libs/hooks/loggerHook"
import useNotification from "@asmr/libs/hooks/notificationHook"
import useTheme from "@asmr/libs/hooks/themeHook"
import useProgress from "@asmr/libs/hooks/progressHook"

function DeveloperMenu(): JSX.Element {
	const authentication = useAuthentication()
	const errorHandler = useErrorHandler()
	const logger = useLogger(DeveloperMenu)
	const navigate = useNavigate()
	const notification = useNotification()
	const [theme, setTheme] = useTheme()
	const [progress, setProgress] = useProgress()

	function onAuthenticationButtonClicked() {
		if (!authentication.isAuthenticated()) {
			logger.info("Authenticated user not detected, redirecting to:", "/authentication/sign-in")
			navigate("/authentication/sign-in")
			return
		}

		logger.info("Authenticated user detected, redirecting to:", "/authentication/sign-out")
		navigate("/authentication/sign-out")
	}

	function onCopyDefaultPasswordButtonClicked() {
		copy(hexToASCII(base64ToHex("QFNNUi1BZG0xbg==")))
		logger.info("Default Administrator password has been copied to clipboard.")
		notification.success("Default Administrator password has been copied to clipboard.")
	}

	function onSwitchThemeButtonClicked() {
		const newTheme = theme === "dark" ? "light" : "dark"
		logger.info("Color theme set to:", newTheme)
		setTheme(newTheme)
	}

	function onShowProfileButtonClicked() {
		navigate("/dashboard/profile")
	}

	function onThrowError() {
		const error = new Error("This is a fake error thrown by the developer.")
		errorHandler(error)
	}

	function onToggleProgressLoadingButtonClicked() {
		setProgress(!progress.loading)
	}

	useEffect(() => {
		logger.info("Development environment detected. Developer Menu is shown.")
	}, [])

	if (!environment.isDevelopment) {
		return <></>
	}

	return (
		<Fab event="hover" icon={<IoBug />} text="Developer Menu">
			<Action text="Switch Light/Dark Theme" onClick={onSwitchThemeButtonClicked}>
				{theme === "dark" ? <IoMoon /> : <IoMoonOutline />}
			</Action>
			<Action text="Toggle Progress Loading" onClick={onToggleProgressLoadingButtonClicked}>
				<IoCog />
			</Action>
			<Action text="Throw Error" onClick={onThrowError}>
				<IoWarning />
			</Action>
			<Action
				text={authentication.isAuthenticated() ? "Sign Out" : "Sign In"}
				onClick={onAuthenticationButtonClicked}
			>
				{authentication.isAuthenticated() ? <IoLogOutOutline /> : <IoLogInOutline />}
			</Action>
			{!authentication.isAuthenticated() && (
				<Action text="Copy Default Administrator Password" onClick={onCopyDefaultPasswordButtonClicked}>
					<IoKeyOutline />
				</Action>
			)}
			{authentication.isAuthenticated() && (
				<Action text="Show Profile" onClick={onShowProfileButtonClicked}>
					<IoPersonOutline />
				</Action>
			)}
		</Fab>
	)
}

export default DeveloperMenu
