import { useEffect } from "react"
import config from "asmr/libs/common/config"

function useDocumentTitle(title?: string) {
	useEffect(() => {
		const applicationName = config.application.name
		document.title = title ? `${applicationName}: ${title}` : applicationName
		return () => {
			document.title = applicationName
		}
	}, [])
}

export default useDocumentTitle
