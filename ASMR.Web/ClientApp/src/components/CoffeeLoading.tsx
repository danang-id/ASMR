import useTheme from "asmr/libs/hooks/themeHook"

import CoffeeLoadingAnimation from "asmr/assets/coffee-loading.gif"
import CoffeeLoadingInvertedAnimation from "asmr/assets/coffee-loading-inverted.gif"

function CoffeeLoading(): JSX.Element {
	const [theme] = useTheme()

	return theme === "dark" ? (
		<img alt="Please wait..." className="coffee-loading" src={CoffeeLoadingInvertedAnimation} />
	) : (
		<img alt="Please wait..." className="coffee-loading" src={CoffeeLoadingAnimation} />
	)
}

export default CoffeeLoading
