import useProgress from "@asmr/libs/hooks/progressHook"
import { createServices } from "@asmr/services"
import { useEffect } from "react"

function useServices() {
	const progressContextInfo = useProgress()
	const services = createServices(new AbortController(), progressContextInfo[1])
	useEffect(() => {
		return () => {
			services.abort()
		}
	}, [])
	return services
}

export default useServices
