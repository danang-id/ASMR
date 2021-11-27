import { ReactNode } from "react"
import { Breakpoint } from "asmr/libs/application/BreakpointContextInfo"
import useBreakpoint from "asmr/libs/hooks/breakpointHook"

interface BreakpointProps {
	max?: Breakpoint
	min?: Breakpoint
	children: ReactNode
}

function BreakpointRenderer({ max = "3xl", min = "xs", children }: BreakpointProps): JSX.Element {
	const { current, includes } = useBreakpoint()
	const shouldRender = includes(min) && (!includes(max) || current === max)

	return <>{shouldRender ? children : void 0}</>
}

export default BreakpointRenderer
