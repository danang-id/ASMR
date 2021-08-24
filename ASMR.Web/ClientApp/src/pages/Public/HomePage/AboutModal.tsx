
import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import config from "@asmr/libs/common/config"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface AboutModalProps {
	onClose: () => void
	onTryAsmr: () => void
	show: boolean
}
function AboutModal({ onClose, onTryAsmr, show }: AboutModalProps): JSX.Element {
	return (
		<Modal onClose={onClose} show={show} title={`What is ${config.application.name}?`}>
			<Modal.Body>
				<strong>{config.application.name}</strong> is your solution for coffee beans management.
				You can manage your bean inventory, track down green bean to roasted bean production,
				and manage roasted bean transaction. You can also see the production report to see how
				well your bean is being roasted.
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button size="sm" onClick={onClose} style="none">I got it!</Button>
					<Button size="sm" onClick={onTryAsmr}>Try Now</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default AboutModal
