import Image from "asmr/components/Image"
import Bean from "asmr/core/entities/Bean"
import "asmr/components/styles/BeanImage.css"

import AsmrLogoImage from "asmr/assets/asmr-logo.webp"

interface BeanImageProps {
	bean?: Bean | null
	clickable?: boolean
}

function BeanImage({ bean, clickable = false }: BeanImageProps): JSX.Element {
	if (!bean) {
		return <></>
	}

	const beanImage = <Image className="bean-image" source={bean.image} fallback={AsmrLogoImage} alt={bean.name} />

	return clickable ? (
		<a href={bean.image} target="_blank">
			{beanImage}
		</a>
	) : (
		beanImage
	)
}

export default BeanImage
