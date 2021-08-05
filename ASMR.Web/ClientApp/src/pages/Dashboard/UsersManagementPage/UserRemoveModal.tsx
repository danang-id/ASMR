
import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import User from "@asmr/data/models/User"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface UserRemoveModalProps {
	onClose: () => void
	onRemoveUser: () => void
	progress: ProgressInfo,
	show: boolean
	user: User | null
}
function UserRemoveModal({ onClose, onRemoveUser, progress, show, user }: UserRemoveModalProps): JSX.Element {
	return (
		<Modal onClose={onClose} show={show} title={`Remove ${user?.firstName ?? ""} ${user?.lastName ?? ""}`}>
			<Modal.Body>
				<p className="remove-user-modal-body">
					Are you sure you would like to remove{" "}
					the user <span className="full-name">{user?.firstName} {user?.lastName}</span>{" "}
					with username <span className="username">{user?.username}</span>{" "}
					from the user list?
				</p>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} outline size="sm" onClick={onClose}>Cancel</Button>
					<Button disabled={progress.loading} size="sm" onClick={onRemoveUser}>Remove</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default UserRemoveModal
