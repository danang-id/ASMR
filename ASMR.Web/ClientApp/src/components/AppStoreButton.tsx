import React from "react"
import "@asmr/components/styles/AppStoreButton.css"

import AppStoreButtonImage from "@asmr/assets/app-store-button.png"

interface AppStoreButtonProps {
	link: string
}

function AppStoreButton({ link }: AppStoreButtonProps): JSX.Element {
	return (
		<a className="app-store-button" href={link}>
			<img alt="Download on the App Store" src={AppStoreButtonImage} />
		</a>
	)
}

export default AppStoreButton
