import { useState, useEffect } from "react"
import CoffeeLoading from "asmr/components/CoffeeLoading"
import BaseLayout from "asmr/layouts/BaseLayout"
import useLogger from "asmr/libs/hooks/loggerHook"
import useProgress from "asmr/libs/hooks/progressHook"
import "asmr/pages/Misc/SuspenseFallbackPage/SuspenseFallbackPage.scoped.css"

const loadingTexts = [
	"Your coffee is on the way",
	"We are making your coffee",
	"Heating up the stove",
	"Making sure the beans are brewed well",
	"Have a coffee while you are waiting",
]

function SuspenseFallbackPage(): JSX.Element {
	const [loadingTextIndex, setLoadingTextIndex] = useState(0)
	const [loadingTick, setLoadingTick] = useState("")
	const logger = useLogger(SuspenseFallbackPage)
	const setProgress = useProgress()[1]

	function onLoadingIntervalTick() {
		if (loadingTick.length === 3) {
			setRandomLoadingText()
			return
		}

		setLoadingTick(loadingTick + ".")
	}

	function setRandomLoadingText() {
		let newLoadingTextIndex = Math.floor(Math.random() * loadingTexts.length)
		while (newLoadingTextIndex === loadingTextIndex) {
			newLoadingTextIndex = Math.floor(Math.random() * loadingTexts.length)
		}
		setLoadingTextIndex(newLoadingTextIndex)
		setLoadingTick("")
	}

	useEffect(() => {
		logger.info("Waiting for Component to be ready...")
		setProgress(true)

		setRandomLoadingText()

		return () => {
			setProgress(false)
			logger.info("Component ready")
		}
	}, [])

	useEffect(() => {
		const timeout = setTimeout(onLoadingIntervalTick, 1000)
		return () => {
			clearTimeout(timeout)
		}
	}, [loadingTick])

	return (
		<BaseLayout className="screen">
			<div className="loading-animation">
				<CoffeeLoading />
			</div>
			<div className="loading-text-wrapper">
				<p>
					{loadingTexts[loadingTextIndex]}
					{loadingTick}
				</p>
			</div>
		</BaseLayout>
	)
}

export default SuspenseFallbackPage
