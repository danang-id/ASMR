import * as React from "react"
import GooglePlayButtonImage from "@asmr/assets/google-play-button.png"

interface GooglePlayButtonProps {
	link: string
}

function GooglePlayButton({ link }: GooglePlayButtonProps): JSX.Element {
	return (
		<a href={link}>
			<div className="google-play-button">
				<img alt="Get it on Google Play" src={GooglePlayButtonImage} />
			</div>
		</a>
	)
}

export default GooglePlayButton
