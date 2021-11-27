import Image from "asmr/components/Image"
import User from "asmr/core/entities/User"
import "asmr/components/styles/UserImage.css"

import AsmrLogoImage from "asmr/assets/asmr-logo.webp"
import { combineClassNames } from "asmr/libs/common/styles"

interface UserImageProps {
	circular?: boolean
	clickable?: boolean
	image?: string | ArrayBuffer | null
	user?: User
}

function UserImage({ circular = false, clickable = false, image, user }: UserImageProps): JSX.Element {
	if (!image && !user) {
		throw new Error(`Either "image" or "user" props is required for UserImage component.`)
	}

	const className = combineClassNames("user-image", circular ? "user-image-circular" : void 0)
	const source = image ?? user?.image
	const alt = user ? `${user.firstName} ${user.lastName}` : "User Image"

	const userImage = <Image className={className} source={source as string} fallback={AsmrLogoImage} alt={alt} />

	return clickable && user?.image ? (
		<a href={user.image} target="_blank">
			{userImage}
		</a>
	) : (
		userImage
	)
}

export default UserImage
