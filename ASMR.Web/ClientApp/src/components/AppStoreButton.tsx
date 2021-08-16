import * as React from "react"
import AppStoreButtonImage from "@asmr/assets/app-store-button.png"

interface AppStoreButtonProps {
	link: string
}

function AppStoreButton({ link }: AppStoreButtonProps): JSX.Element {
	return (
		<a href={link}>
			<div className="google-play-button">
				<img alt="Download on the App Store" src={AppStoreButtonImage} />
			</div>
		</a>
	)
}

export default AppStoreButton
