import { useContext } from "react"
import NetworkContext from "@asmr/libs/application/NetworkContext"

function useNetwork() {
	return useContext(NetworkContext)
}

export default useNetwork
