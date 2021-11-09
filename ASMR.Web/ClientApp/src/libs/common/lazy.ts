import { lazy as reactLazy, ComponentType, LazyExoticComponent } from "react"
import useLogger from "@asmr/libs/hooks/loggerHook"

function lazy<T extends ComponentType<any>>(factory: () => Promise<{ default: T }>): LazyExoticComponent<T> {
	const logger = useLogger("Lazy Component")
	return reactLazy(async () => {
		const component = await factory()
		logger.info("Component loaded:", component.default.name)
		return component
	})
}

export default lazy
