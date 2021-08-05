import { ReactNode, useEffect, useState } from "react"
import ThemeContext from "@asmr/libs/application/ThemeContext"
import { Theme } from "@asmr/libs/application/ThemeContextInfo"
import useLogger from "@asmr/libs/hooks/loggerHook"

interface ThemeProviderProps {
	children: ReactNode
}

function ThemeProvider({ children }: ThemeProviderProps): JSX.Element {
	const logger = useLogger(ThemeProvider)
	const [mediaQueryList, setMediaQueryList] = useState<MediaQueryList | null>(null)
	const [theme, setTheme] = useState<Theme>("light")

	function onMediaMatches(matches: boolean) {
		setTheme(matches ? "dark" : "light")
	}

	function onMediaQueryListChanged(event: MediaQueryListEvent) {
		if (event.type === "change") {
			onMediaMatches(event.matches)
		}
	}

	useEffect(() => {
		if ("matchMedia" in window) {
			setMediaQueryList(window.matchMedia("(prefers-color-scheme: dark)"))
		}
	}, [])

	useEffect(() => {
		if (mediaQueryList !== null) {
			onMediaMatches(mediaQueryList.matches)
			if ("addEventListener" in mediaQueryList) {
				mediaQueryList.addEventListener("change", onMediaQueryListChanged)
			} else {
				// eslint-disable-next-line @typescript-eslint/ban-ts-comment
				// @ts-ignore
				// noinspection JSDeprecatedSymbols
				mediaQueryList.addListener(onMediaQueryListChanged)
			}
		}
		return () => {
			if (mediaQueryList !== null) {
				if ("removeEventListener" in mediaQueryList) {
					mediaQueryList.removeEventListener("change", onMediaQueryListChanged)
				} else {
					// eslint-disable-next-line @typescript-eslint/ban-ts-comment
					// @ts-ignore
					// noinspection JSDeprecatedSymbols
					mediaQueryList.removeListener(onMediaQueryListChanged)
				}
			}
		}
	}, [mediaQueryList])

	useEffect(() => {
		logger.info("Theme:", theme)
		if (theme === "dark") {
			document.documentElement.classList.add("dark")
		} else {
			document.documentElement.classList.remove("dark")
		}
	}, [theme])

	return (
		<ThemeContext.Provider value={[theme, setTheme]}>
			{children}
		</ThemeContext.Provider>
	)
}

export default ThemeProvider
