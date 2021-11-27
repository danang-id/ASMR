import Image from "asmr/components/Image"
import config from "asmr/libs/common/config"
import { combineClassNames } from "asmr/libs/common/styles"
import useTheme from "asmr/libs/hooks/themeHook"
import "asmr/components/styles/ApplicationLogo.css"

import AsmrLogoImage from "asmr/assets/asmr-logo.webp"
import AsmrLogoFallbackImage from "asmr/assets/asmr-logo.png"
import AsmrLogoInvertedImage from "asmr/assets/asmr-logo-inverted.webp"
import AsmrLogoInvertedFallbackImage from "asmr/assets/asmr-logo-inverted.png"

interface ApplicationLogoProps {
	className?: string
	allowTheme?: boolean
}

function ApplicationLogo({ className, allowTheme = true, ...props }: ApplicationLogoProps): JSX.Element {
	const [theme] = useTheme()
	const useInvertedImage = allowTheme && theme === "dark"

	const imageSource = useInvertedImage ? AsmrLogoInvertedImage : AsmrLogoImage
	const fallbackImageSource = useInvertedImage ? AsmrLogoInvertedFallbackImage : AsmrLogoFallbackImage

	className = combineClassNames(className, "application-logo")

	return (
		<Image
			className={className}
			source={imageSource}
			type="image/webp"
			fallback={fallbackImageSource}
			alt={config.application.name}
			{...props}
		/>
	)
}

export default ApplicationLogo
