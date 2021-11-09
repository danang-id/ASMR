import { ComponentType, MouseEvent, ReactNode } from "react"
import { IoCogOutline } from "react-icons/io5"
import { combineClassNames } from "@asmr/libs/common/styles"
import useProgress from "@asmr/libs/hooks/progressHook"
import "@asmr/components/styles/Button.css"

interface ButtonProps {
	children?: ReactNode
	className?: string
	disabled?: boolean
	icon?: ComponentType<any>
	onClick?: (event: MouseEvent<HTMLButtonElement>) => void
	style?: "danger" | "filled" | "none" | "outline"
	size?: "xs" | "sm" | "md" | "lg"
	type?: "button" | "reset" | "submit"
}

function Button({
	children,
	className,
	icon: Icon,
	size = "md",
	style = "filled",
	...props
}: ButtonProps): JSX.Element {
	className = combineClassNames(className, "button", `button-${size}`, `button-${style}`)
	const [progress] = useProgress()

	const ButtonIcon = () =>
		progress.loading ? <IoCogOutline className="animate-spin" /> : Icon ? <Icon className="button-icon" /> : <></>

	return (
		<button className={className} {...props}>
			{Icon && <ButtonIcon />}
			{Icon && children && <>&nbsp;&nbsp;</>}
			{children}
		</button>
	)
}

export default Button
