import { createContext } from "react"
import BreakpointContextInfo from "asmr/libs/application/BreakpointContextInfo"

const BreakpointContext = createContext<BreakpointContextInfo>({
	activeList: [],
	current: "xs",
	includes: () => false,
	width: -1,
})

export default BreakpointContext
