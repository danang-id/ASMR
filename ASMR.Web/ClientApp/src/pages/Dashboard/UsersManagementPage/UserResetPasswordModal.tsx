import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import User from "@asmr/data/models/User"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface UserResetPasswordModalProps {
	onClose: () => void
	onResetUserPassword: () => void
	progress: ProgressInfo
	show: boolean
	user: User | null
}
function UserUpdatePasswordModal({
	onClose,
	onResetUserPassword,
	progress,
	show,
	user,
}: UserResetPasswordModalProps): JSX.Element {
	return (
		<Modal
			onClose={onClose}
			show={show}
			title={`Reset Password for ${user?.firstName ?? ""} ${user?.lastName ?? ""}`}
		>
			<Modal.Body>
				{user?.isEmailConfirmed ? (
					<div className="reset-user-password-modal-body">
						<p>
							We will sent an instruction to reset the account password to{" "}
							<span className="email-address">{user?.emailAddress}</span>.
						</p>
						<br />
						<p>Are you sure you would like to continue?</p>
					</div>
				) : (
					<div className="reset-user-password-modal-body">
						<p>
							The email address <span className="email-address">{user?.emailAddress}</span> has not been
							confirmed. Password reset procedure cannot continue.
						</p>
					</div>
				)}
			</Modal.Body>

			<Modal.Footer>
				{user?.isEmailConfirmed ? (
					<div className="modal-actions">
						<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
							Cancel
						</Button>
						<Button disabled={progress.loading} size="sm" onClick={() => onResetUserPassword()}>
							Send Reset Instruction
						</Button>
					</div>
				) : (
					<div className="modal-actions">
						<Button disabled={progress.loading} size="sm" onClick={onClose}>
							Okay
						</Button>
					</div>
				)}
			</Modal.Footer>
		</Modal>
	)
}

export default UserUpdatePasswordModal
