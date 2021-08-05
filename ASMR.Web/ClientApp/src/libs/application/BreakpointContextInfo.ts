export type Breakpoint = "xs" | "sm" | "md" | "lg" | "xl" | "2xl" | "3xl"
export type BreakpointContextInfo = {
	activeList: Breakpoint[]
	current: Breakpoint
	includes: (breakpoint: Breakpoint) => boolean
	width: number
}

export default BreakpointContextInfo
