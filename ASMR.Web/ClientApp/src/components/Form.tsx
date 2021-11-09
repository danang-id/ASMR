import { ChangeEvent, FormEvent, ReactNode } from "react"
import { combineClassNames } from "@asmr/libs/common/styles"
import useProgress from "@asmr/libs/hooks/progressHook"
import "@asmr/components/styles/Form.css"

export interface FormProps {
	children?: ReactNode
	className?: string
	onSubmit?: (event: FormEvent<HTMLFormElement>) => void
}

function Form({ children, className, onSubmit }: FormProps): JSX.Element {
	className = combineClassNames(className, "form")
	return (
		<form className={className} onSubmit={onSubmit}>
			{children}
		</form>
	)
}

export interface FormInputProps {
	accept?: string
	checked?: boolean
	className?: string
	defaultValue?: string
	disabled?: boolean
	name?: string
	onChange?: (event: ChangeEvent<HTMLInputElement>) => void
	placeholder?: string
	readOnly?: boolean
	required?: boolean
	type?:
		| "text"
		| "password"
		| "email"
		| "number"
		| "url"
		| "date"
		| "datetime-local"
		| "month"
		| "week"
		| "time"
		| "search"
		| "tel"
		| "checkbox"
		| "radio"
		| "file"
	value?: string
}

Form.Input = function ({ className, disabled = false, ...props }: FormInputProps): JSX.Element {
	const [progress] = useProgress()
	className = combineClassNames(className, "form-input", "form-input-custom")
	return <input className={className} disabled={disabled || progress.loading} {...props} />
}

export interface FormCheckBoxProps {
	checked?: boolean
	children: ReactNode
	className?: string
	defaultValue?: string
	disabled?: boolean
	name?: string
	onChange?: (event: ChangeEvent<HTMLInputElement>) => void
	readOnly?: boolean
	value?: string
}

Form.CheckBox = function ({ children, className, disabled = false, name, ...props }: FormCheckBoxProps): JSX.Element {
	const [progress] = useProgress()
	className = combineClassNames(className, "form-checkbox", "form-checkbox-custom")
	return (
		<label className="form-checkbox-container" htmlFor={name}>
			<input
				className={className}
				disabled={disabled || progress.loading}
				name={name}
				type="checkbox"
				{...props}
			/>
			<span className="form-checkbox-label">{children}</span>
		</label>
	)
}

export interface FormSelectProps {
	children?: ReactNode
	className?: string
	multiple?: boolean
}

Form.Select = function ({ children, className, ...props }: FormSelectProps): JSX.Element {
	className = combineClassNames(className, "form-select", "form-select-custom")
	return (
		<select className={className} {...props}>
			{children}
		</select>
	)
}

export interface FormTextAreaProps {
	children?: ReactNode
	className?: string
	name?: string
	onChange?: (event: ChangeEvent<HTMLTextAreaElement>) => void
	rows?: number
	value?: string
}

Form.TextArea = function ({ children, className, rows, ...props }: FormTextAreaProps): JSX.Element {
	className = combineClassNames(className, "form-text-area", "form-text-area-custom")

	return (
		<textarea className={className} rows={rows} {...props}>
			{children}
		</textarea>
	)
}

export default Form
