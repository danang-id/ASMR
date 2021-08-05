import { ReactNode, useEffect, useState } from "react"
import ProgressContext from "@asmr/libs/application/ProgressContext"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import useLogger from "@asmr/libs/hooks/loggerHook"

interface ProgressProviderProps {
	children: ReactNode
}

function ProgressProvider({ children }: ProgressProviderProps): JSX.Element {
	const logger = useLogger(ProgressProvider)
	const [progressInfo, setProgressInfo] = useState<ProgressInfo>({
		loading: false,
		percentage: 0
	})

	function setProgress(loading: boolean, percentage: number = 0) {
		if (percentage <= 0) {
			percentage = 0
		}
		if (percentage >= 1) {
			percentage = 1
		}
		setProgressInfo({ loading, percentage: percentage })
	}
	
	useEffect(() => {
		const { loading, percentage } = progressInfo
		logger.info("Loading:", loading, "Percentage:", percentage)
	}, [progressInfo])

	return (
		<ProgressContext.Provider value={[progressInfo, setProgress]}>
			{children}
		</ProgressContext.Provider>
	)
}

export default ProgressProvider
