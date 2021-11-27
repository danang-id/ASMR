import { Fragment, ReactNode } from "react"
import { Listbox, Transition } from "@headlessui/react"
import { IoCaretDownOutline, IoCheckmark } from "react-icons/io5"
import "asmr/components/styles/ListSelector.css"

export interface ListSelectorProps<T = unknown> {
	children: ReactNode
	emptyOption?: string
	onChange: (value: T) => void
	value: T
	valueRenderer?: (value: T) => string
}

function ListSelector<T = unknown>({
	children,
	emptyOption,
	onChange,
	value,
	valueRenderer,
}: ListSelectorProps<T>): JSX.Element {
	return (
		<Listbox onChange={onChange} value={value}>
			<div className="list-selector">
				<Listbox.Button className="selector-button">
					<span className="selector-button-value">
						{valueRenderer && typeof valueRenderer === "function" ? valueRenderer(value) : value}
					</span>
					<span className="selector-button-icon">
						<IoCaretDownOutline className="w-5 h-5" aria-hidden />
					</span>
				</Listbox.Button>
				<Transition
					as={Fragment}
					leave="transition ease-in duration-100"
					leaveFrom="opacity-100"
					leaveTo="opacity-0"
				>
					<Listbox.Options className="selector-options">
						{children ? (
							children
						) : emptyOption ? (
							<ListOption disabled index={0} value={emptyOption} />
						) : (
							children
						)}
					</Listbox.Options>
				</Transition>
			</div>
		</Listbox>
	)
}

export interface ListOptionProps<T = unknown> {
	disabled?: boolean
	index: number
	value: T
	valueRenderer?: (value: T) => string
}

export function ListOption<T = unknown>({ disabled, index, value, valueRenderer }: ListOptionProps<T>): JSX.Element {
	return (
		<Listbox.Option
			disabled={disabled}
			key={index}
			className={({ active }) => `list-option ${active ? "list-option-active" : ""}`}
			value={value}
		>
			{({ selected }) => (
				<>
					<span className={`list-option-value ${selected ? "list-option-selected" : ""} `}>
						{valueRenderer && typeof valueRenderer === "function" ? valueRenderer(value) : value}
					</span>
					{selected && (
						<span className="list-option-icon">
							<IoCheckmark className="w-5 h-5" aria-hidden="true" />
						</span>
					)}
				</>
			)}
		</Listbox.Option>
	)
}

export default ListSelector
