
import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import Bean from "@asmr/data/models/Bean"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/BeanManagementPage/BeansManagementModal.scoped.css"

interface BeanRemoveModalProps {
	bean: Bean | null
	onClose: () => void
	onRemoveBean: () => void
	progress: ProgressInfo,
	show: boolean
}
function BeanRemoveModal({ bean, onClose, onRemoveBean, progress, show }: BeanRemoveModalProps): JSX.Element {
	return (
		<Modal onClose={onClose} show={show} title={`Remove ${bean?.name ?? ""}`}>
			<Modal.Body>
				<p className="remove-bean-modal-body">
					Are you sure you would like to remove{" "}
					the bean <span className="name">{bean?.name ?? ""}</span>{" "}
					from the bean list?</p>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>Cancel</Button>
					<Button disabled={progress.loading} style="danger" size="sm" onClick={onRemoveBean}>Remove</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default BeanRemoveModal
