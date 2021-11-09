export function singleSwitchToggle(trueSetter?: (value: boolean) => void, falseSetter?: ((value: boolean) => void)[]) {
	return new Promise<void>((resolve) => {
		if (falseSetter && Array.isArray(falseSetter)) {
			for (const modalSetterFalse of falseSetter) {
				modalSetterFalse(false)
			}
		}
		if (trueSetter) {
			setTimeout(() => {
				trueSetter(true)
				resolve()
			})
		}
	})
}
