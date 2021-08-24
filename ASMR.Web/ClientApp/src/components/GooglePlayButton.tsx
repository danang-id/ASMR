import * as React from "react"
import "@asmr/components/styles/GooglePlayButton.css"

import GooglePlayButtonImage from "@asmr/assets/google-play-button.png"

interface GooglePlayButtonProps {
	link: string
}

function GooglePlayButton({ link }: GooglePlayButtonProps): JSX.Element {
	return (
		<a className="google-play-button" href={link}>
			<img alt="Get it on Google Play" src={GooglePlayButtonImage} />
		</a>
	)
}

export default GooglePlayButton
