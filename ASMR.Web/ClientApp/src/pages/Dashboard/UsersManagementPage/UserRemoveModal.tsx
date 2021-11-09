import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import User from "@asmr/data/models/User"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface UserRemoveModalProps {
	onClose: () => void
	onRemoveUser: () => void
	progress: ProgressInfo
	show: boolean
	user: User | null
}
function UserRemoveModal({ onClose, onRemoveUser, progress, show, user }: UserRemoveModalProps): JSX.Element {
	return (
		<Modal onClose={onClose} show={show} title={`Remove ${user?.firstName ?? ""} ${user?.lastName ?? ""}`}>
			<Modal.Body>
				<div className="remove-user-modal-body">
					<p>
						The following user account will be removed.
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
					<Button disabled={progress.loading} style="danger" size="sm" onClick={onRemoveUser}>
						Remove
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default UserRemoveModal
