import { useEffect } from "react"

function useInit(initHandler: () => void | Promise<void> | (() => void), useExitHandler: boolean = false) {
	useEffect(() => {
		const exitHandler = initHandler()
		if (typeof exitHandler === "function" && useExitHandler) {
			return exitHandler
		}
	}, [])
}

export default useInit
