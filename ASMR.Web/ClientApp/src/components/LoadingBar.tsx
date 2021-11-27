import { useEffect, useState } from "react"
import useProgress from "asmr/libs/hooks/progressHook"
import "asmr/components/styles/LoadingBar.css"

interface LoadingBarProps {
	showSpinner?: boolean
}

function LoadingBar({ showSpinner = false }: LoadingBarProps): JSX.Element {
	const [appearDelayWidth, setAppearDelayWidth] = useState(0)
	const [disappearDelayHide, setDisappearDelayHide] = useState(false)
	const [size, setSize] = useState(0)
	const [progress, setProgress] = useProgress()

	function calculatePercentage(percentage: number) {
		percentage = percentage ? percentage : 0
		let random: number

		if (percentage >= 0 && percentage < 0.25) {
			random = (Math.random() * (5 - 3 + 1) + 10) / 100
		} else if (percentage >= 0.25 && percentage < 0.65) {
			random = (Math.random() * 3) / 100
		} else if (percentage >= 0.65 && percentage < 0.9) {
			random = (Math.random() * 2) / 100
		} else if (percentage >= 0.9 && percentage < 0.99) {
			random = 0.005
		} else {
			random = 0
		}

		percentage += random
		return percentage
	}

	function getBarStyle() {
		return {
			width: appearDelayWidth ? 0 : `${progress.percentage * 100}%`,
			display: disappearDelayHide || progress.percentage > 0 ? "block" : "none",
		}
	}

	function getSpinnerIconStyle() {
		return {
			display: size > 0 ? "block" : "none",
		}
	}

	function hide() {
		if (size - 1 < 0) {
			setSize(0)
			return
		}

		setSize(0)
		setDisappearDelayHide(true)
		setProgress(progress.loading, 1)

		setTimeout(() => {
			setDisappearDelayHide(false)
			setProgress(progress.loading, 0)
		}, 500)
	}

	function show() {
		const percentage = calculatePercentage(progress.percentage)
		setSize(size + 1)
		setAppearDelayWidth(size === 0 ? 0 : 1)
		setProgress(progress.loading, percentage)

		if (appearDelayWidth) {
			setTimeout(() => {
				setAppearDelayWidth(0)
			})
		}
	}

	useEffect(() => {
		if (progress.loading) {
			show()
			return
		}

		hide()
	}, [progress.loading])

	return (
		<>
			<div className="loading-bar-main">
				<div className="bar" style={getBarStyle()}>
					<div className="progress" />
				</div>
			</div>
			{showSpinner && (
				<div className="loading-bar-spinner">
					<div className="icon" style={getSpinnerIconStyle()} />
				</div>
			)}
		</>
	)
}

export default LoadingBar
