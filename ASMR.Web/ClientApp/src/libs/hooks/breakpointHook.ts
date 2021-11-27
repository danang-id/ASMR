import { useContext } from "react"
import BreakpointContext from "asmr/libs/application/BreakpointContext"

function useBreakpoint() {
	return useContext(BreakpointContext)
}

export default useBreakpoint
