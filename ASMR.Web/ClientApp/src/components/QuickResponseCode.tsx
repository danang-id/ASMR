import QRCode from "react-qr-code"
// import useTheme from "@asmr/libs/hooks/themeHook"
import "@asmr/components/styles/QRCode.css"

interface QuickResponseCodeProps {
	level?: "L" | "M" | "Q" | "H"
	size?: number
	value: string
}

function QuickResponseCode({ size = 128, ...props}: QuickResponseCodeProps): JSX.Element {
	// const [theme] = useTheme()

	return (
		<div className="quick-response-code">
			<QRCode size={size} {...props}/>
		</div>
	)
}

export default QuickResponseCode
