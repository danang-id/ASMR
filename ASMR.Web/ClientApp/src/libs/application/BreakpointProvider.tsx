import { ReactNode, useEffect, useState } from "react"
import BreakpointContext from "@asmr/libs/application/BreakpointContext"
import { Breakpoint } from "@asmr/libs/application/BreakpointContextInfo"
import tailwind from "@asmr/libs/common/tailwind"
import useLogger from "@asmr/libs/hooks/loggerHook"

interface BreakpointProviderProps {
	children: ReactNode
}

function BreakpointProvider({ children }: BreakpointProviderProps): JSX.Element {
	const logger = useLogger(BreakpointProvider)
	const [activeList, setActiveList] = useState<Breakpoint[]>([])
	const [current, setCurrent] = useState<Breakpoint>("xs")
	const [width, setWidth] = useState(window.innerWidth ? window.innerWidth : 0)

	function onResize(event: Event) {
		if (event.type !== "resize" && window.innerWidth === width) {
			return
		}

		setWidth(window.innerWidth)
	}

	function includes(breakpoint: Breakpoint) {
		return activeList.includes(breakpoint)
	}

	useEffect(() => {
		window.addEventListener("resize", onResize)
		return () => {
			window.removeEventListener("resize", onResize)
		}
	}, [])

	useEffect(() => {
		const breakpointList = activeList.map((value) => (value === current ? `[${value}]` : value)).join(" ")
		logger.info("Breakpoint changed:", breakpointList)
	}, [current])

	useEffect(() => {
		setActiveList(tailwind.getActiveBreakpoints(width) as Breakpoint[])
		setCurrent(tailwind.getCurrentBreakpoint(width) as Breakpoint)
	}, [width])

	return (
		<BreakpointContext.Provider value={{ activeList, current, includes, width }}>
			{children}
		</BreakpointContext.Provider>
	)
}

export default BreakpointProvider
