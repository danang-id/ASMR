import * as localforage from "localforage"
import { useState } from "react"
import useInit from "@asmr/libs/hooks/initHook"
import useLogger from "@asmr/libs/hooks/loggerHook"

const dataInstance = localforage.createInstance({
	name: "asmr",
	description: "Persisted Application State",
	storeName: "persisted-state"
})

function usePersistedState<T = unknown>(name: string, initialValue?: T): [T | undefined, (value?: T) => void] {
	useInit(onInit)
	const logger = useLogger(usePersistedState)
	const [state, setLocalState] = useState<T | undefined>()

	async function onInit() {
		const persistedState = await dataInstance.getItem<T>(name)
		if (typeof persistedState !== "undefined" && persistedState !== null) {
			setLocalState(persistedState)
		} else if (typeof initialValue !== "undefined" && initialValue !== null) {
			await setPersistedState(initialValue)
		}
	}

	async function setPersistedState(value?: T) {
		if (typeof value !== "undefined" && value !== null) {
			const persistedState = await dataInstance.setItem<T>(name, value)
			setLocalState(persistedState)
		} else {
			await dataInstance.removeItem(name)
			setLocalState(void 0)
		}
	}

	function setState(value?: T) {
		setPersistedState(value).then().catch(logger.error)
	}

	return [state, setState]
}

export default usePersistedState
