import { createContext } from "react"
import NetworkContextInfo from "asmr/libs/application/NetworkContextInfo"

const NetworkContext = createContext<NetworkContextInfo>({
	onLine: true,
})

export default NetworkContext
