import useProgress from "asmr/libs/hooks/progressHook"
import { createServices } from "asmr/services"
import { useEffect } from "react"

function useServices() {
	const progressContextInfo = useProgress()
	const services = createServices(progressContextInfo[1])
	useEffect(() => {
		return () => {
			services.abort("Page changed, aborting service request...")
		}
	}, [])
	return services
}

export default useServices
