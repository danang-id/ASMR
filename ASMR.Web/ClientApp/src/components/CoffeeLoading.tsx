
import Image from "@asmr/components/Image"
import useTheme from "@asmr/libs/hooks/themeHook"

import CoffeeLoadingDarkAnimation from "@asmr/assets/coffee-loading-dark.webp"
import CoffeeLoadingDarkFallbackAnimation from "@asmr/assets/coffee-loading-dark.gif"
import CoffeeLoadingLightAnimation from "@asmr/assets/coffee-loading-light.webp"
import CoffeeLoadingLightFallbackAnimation from "@asmr/assets/coffee-loading-light.gif"

const CoffeeLoadingDark = () => (
	<Image alt="Please wait..."
			className="coffee-loading"
			source={CoffeeLoadingDarkAnimation}
			type="image/webp"
			fallback={CoffeeLoadingDarkFallbackAnimation}
	/>
)

const CoffeeLoadingLight = () => (
	<Image alt="Please wait..."
			className="coffee-loading"
			source={CoffeeLoadingLightAnimation}
			type="image/webp"
			fallback={CoffeeLoadingLightFallbackAnimation}
	/>
)

function CoffeeLoading(): JSX.Element {
	const [theme] = useTheme()

	return theme === "dark" ? <CoffeeLoadingDark/> : <CoffeeLoadingLight/>
}

export default CoffeeLoading
