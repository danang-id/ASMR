import resolveConfig from "tailwindcss/resolveConfig"
import { TailwindConfig } from "tailwindcss/tailwind-config"

const fullConfig = resolveConfig({} as TailwindConfig)

export function getBreakpointValue(value: string): number {
	if (!fullConfig || !fullConfig.theme || !fullConfig.theme.screens) {
		return -1
	}

	const screens = fullConfig.theme.screens
	return parseInt(screens[value].slice(0, screens[value].indexOf("px")))
}

export function getActiveBreakpoints(width = window.innerWidth): string[] {
	if (!fullConfig || !fullConfig.theme || !fullConfig.theme.screens) {
		return []
	}

	const activeBreakpoints: string[] = [ "xs" ]
	for (const breakpoint of Object.keys(fullConfig.theme.screens)) {
		const breakpointValue = getBreakpointValue(breakpoint)
		if (width >= breakpointValue) {
			activeBreakpoints.push(breakpoint)
		}
	}

	return activeBreakpoints
}

export function getCurrentBreakpoint(width = window.innerWidth): string {
	if (!fullConfig || !fullConfig.theme || !fullConfig.theme.screens) {
		return ""
	}

	let activeBreakpoint: string = "xs"
	let biggestBreakpointValue = 0
	for (const breakpoint of Object.keys(fullConfig.theme.screens)) {
		const breakpointValue = getBreakpointValue(breakpoint)
		if (breakpointValue > biggestBreakpointValue && width >= breakpointValue) {
			biggestBreakpointValue = breakpointValue
			activeBreakpoint = breakpoint
		}
	}
	return activeBreakpoint
}


export default { getBreakpointValue, getActiveBreakpoints, getCurrentBreakpoint }
