import { ComponentType, MouseEvent, ReactNode } from "react"
import { combineClassNames } from "@asmr/libs/common/styles"
import "@asmr/components/styles/Button.css"

interface ButtonProps {
	children?: ReactNode
	className?: string
	disabled?: boolean
	icon?: ComponentType<any>
	inverted?: boolean
	onClick?: (event: MouseEvent<HTMLButtonElement>) => void
	outline?: boolean
	size?: "xs" | "sm" | "md" | "lg"
	type?: "button" | "reset" | "submit"
}

function Button({
					children,
					className, icon: Icon,
					inverted = false,
					outline = false,
					size = "md",
					...props }: ButtonProps): JSX.Element {
	className = combineClassNames(className, "button", `button-${size}`)
	if (outline) {
		className = combineClassNames(className, "button-outline")
	} else if (inverted) {
		className = combineClassNames(className, "button-inverted")
	} else {
		className = combineClassNames(className, "button-filled")
	}

	return (
		<button className={className} {...props}>
			{ Icon && <Icon className="button-icon" />}
			{ (Icon && children) && <>&nbsp;&nbsp;</> }
			{children}
		</button>
	)
}

export default Button
