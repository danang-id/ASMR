import { useContext } from "react"
import AuthenticationContext from "asmr/libs/security/AuthenticationContext"

function useAuthentication() {
	return useContext(AuthenticationContext)
}

export default useAuthentication
