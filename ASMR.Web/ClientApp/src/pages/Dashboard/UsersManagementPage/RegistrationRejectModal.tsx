import Button from "asmr/components/Button"
import Modal from "asmr/components/Modal"
import User from "asmr/core/entities/User"
import { ProgressInfo } from "asmr/libs/application/ProgressContextInfo"
import "asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface RegistrationRejectModalProps {
	onClose: () => void
	onRejectRegistration: () => void
	progress: ProgressInfo
	show: boolean
	user: User | null
}
function RegistrationRejectModal({
	onClose,
	onRejectRegistration,
	progress,
	show,
	user,
}: RegistrationRejectModalProps): JSX.Element {
	return (
		<Modal
			onClose={onClose}
			show={show}
			title={`Reject Registration of ${user?.firstName && ""} ${user?.lastName && ""}`}
		>
			<Modal.Body>
				<div className="reject-registration-modal-body">
					<p>
						The following account registration will be rejected.
						<br />
						<span className="full-name">
							{user?.firstName} {user?.lastName}
						</span>
						<br />
						<span className="username">{user?.emailAddress}</span>
					</p>
					<br />
					<p>Are you sure you would like to continue?</p>
				</div>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} style="danger" size="sm" onClick={onRejectRegistration}>
						Reject
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default RegistrationRejectModal
