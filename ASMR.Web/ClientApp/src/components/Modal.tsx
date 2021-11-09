import { CSSProperties, Fragment, ReactNode } from "react"
import { Dialog, Transition } from "@headlessui/react"
import { IoCloseOutline } from "react-icons/io5"
import { combineClassNames } from "@asmr/libs/common/styles"
import "@asmr/components/styles/Modal.css"

export interface ModalProps {
	children?: ReactNode
	className?: string
	onClose: () => void
	show: boolean
	style?: CSSProperties
	title?: string
}

function Modal({ children, className, onClose, show, title, ...props }: ModalProps): JSX.Element {
	className = combineClassNames("modal-container", className)
	return (
		<Transition appear show={show} as={Fragment}>
			<Dialog as="div" className="modal" open={show} onClose={onClose}>
				<div className="modal-dialog">
					<Transition.Child
						as={Fragment}
						enter="ease-out duration-300"
						enterFrom="opacity-0"
						enterTo="opacity-100"
						leave="ease-in duration-200"
						leaveFrom="opacity-100"
						leaveTo="opacity-0"
					>
						<Dialog.Overlay className="modal-overlay" />
					</Transition.Child>

					<span className="modal-center" aria-hidden="true">
						&#8203;
					</span>

					<Transition.Child
						as={Fragment}
						enter="ease-out duration-300"
						enterFrom="opacity-0 scale-95"
						enterTo="opacity-100 scale-100"
						leave="ease-in duration-200"
						leaveFrom="opacity-100 scale-100"
						leaveTo="opacity-0 scale-95"
					>
						<div className={className} {...props}>
							<Dialog.Title className="modal-title" {...props}>
								{title && "Dialog"}
								<button onClick={onClose}>
									<IoCloseOutline />
								</button>
							</Dialog.Title>
							{children}
						</div>
					</Transition.Child>
				</div>
			</Dialog>
		</Transition>
	)
}

export interface ModalBodyProps {
	children?: ReactNode
	className?: string
	style?: CSSProperties
}

Modal.Body = function ({ children, className, ...props }: ModalBodyProps): JSX.Element {
	className = combineClassNames("modal-body", className)
	return (
		<div className={className} {...props}>
			{children}
		</div>
	)
}

export interface ModalFooterProps {
	children?: ReactNode
	className?: string
	style?: CSSProperties
}

Modal.Footer = function ({ children, className, ...props }: ModalFooterProps): JSX.Element {
	className = combineClassNames("modal-footer", className)
	return (
		<div className={className} {...props}>
			{children}
		</div>
	)
}

export default Modal
