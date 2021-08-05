import { ReactNode, useEffect, useState } from "react"
import NetworkContext from "@asmr/libs/application/NetworkContext"
import useLogger from "@asmr/libs/hooks/loggerHook"

interface NetworkProviderProps {
	children: ReactNode
}

function NetworkProvider({ children }: NetworkProviderProps): JSX.Element {
	const logger = useLogger(NetworkProvider)
	const [onLine, setOnLine] = useState<boolean>(
		"navigator" in window  ? window.navigator.onLine : true)

	function onNetworkChanged(event: Event) {
		switch (event.type) {
			case "offline":
				setOnLine(false)
				break
			case "online":
				setOnLine(true)
				break
		}
	}

	useEffect(() => {
		window.addEventListener("offline", onNetworkChanged)
		window.addEventListener("online", onNetworkChanged)
		return () => {
			window.removeEventListener("offline", onNetworkChanged)
			window.removeEventListener("online", onNetworkChanged)
		}
	}, [])

	useEffect(() => {
		if (onLine) {
			logger.info("Network:", "online")
		} else {
			logger.warn("Network:", "offline")
		}
	}, [onLine])

	return (
		<NetworkContext.Provider value={{ onLine }}>
			{children}
		</NetworkContext.Provider>
	)
}

export default NetworkProvider
