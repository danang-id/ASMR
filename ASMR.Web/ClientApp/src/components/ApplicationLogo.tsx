import Image from "@asmr/components/Image"
import config from "@asmr/libs/common/config"
import { combineClassNames } from "@asmr/libs/common/styles"
import useTheme from "@asmr/libs/hooks/themeHook"
import "@asmr/components/styles/ApplicationLogo.css"

import AsmrLogoImage from "@asmr/assets/asmr-logo.webp"
import AsmrLogoFallbackImage from "@asmr/assets/asmr-logo.png"
import AsmrLogoDarkImage from "@asmr/assets/asmr-logo-dark.webp"
import AsmrLogoDarkFallbackImage from "@asmr/assets/asmr-logo-dark.png"
import AsmrLogoLightImage from "@asmr/assets/asmr-logo-light.webp"
import AsmrLogoLightFallbackImage from "@asmr/assets/asmr-logo-light.png"

interface ApplicationLogoProps {
	className?: string
	withBackground?: boolean
}

function ApplicationLogo({ className, withBackground = false, ...props }: ApplicationLogoProps): JSX.Element {
	const [theme] = useTheme()
	const imageSource = withBackground
		? theme === "dark" ? AsmrLogoLightImage : AsmrLogoDarkImage
		: AsmrLogoImage
	const fallbackImageSource = withBackground
		? theme === "dark" ? AsmrLogoLightFallbackImage : AsmrLogoDarkFallbackImage
		: AsmrLogoFallbackImage

	className = combineClassNames(className, "application-logo")
	if (!withBackground) {
		className = combineClassNames(className, "application-logo-with-border")
	}

	return (
		<Image className={className}
				source={imageSource}
				type="image/webp"
				fallback={fallbackImageSource}
				alt={config.application.name}
				{...props}
		/>
	)
}

export default ApplicationLogo
