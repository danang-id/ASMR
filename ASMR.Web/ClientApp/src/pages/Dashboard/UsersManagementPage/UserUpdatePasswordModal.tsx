
import { ChangeEvent, useEffect, useState } from "react"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import Modal from "@asmr/components/Modal"
import User from "@asmr/data/models/User"
import UpdateUserPasswordRequestModel from "@asmr/data/request/UpdateUserPasswordRequestModel"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface UserUpdatePasswordModalProps {
	onClose: () => void
	onUpdateUserPassword: (requestModel: UpdateUserPasswordRequestModel) => void
	progress: ProgressInfo,
	show: boolean
	user: User | null
}
function UserUpdatePasswordModal({ onClose, onUpdateUserPassword, progress, show, user }: UserUpdatePasswordModalProps): JSX.Element {
	const emptyRequestModel: UpdateUserPasswordRequestModel = {
		password: "",
		passwordConfirmation: ""
	}
	const [requestModel, setRequestModal] = useState<UpdateUserPasswordRequestModel>(emptyRequestModel)

	function onChange(event: ChangeEvent<HTMLInputElement>) {
		const newRequestModel = { ...requestModel }
		// @ts-ignore
		newRequestModel[event.target.name] = event.target.value
		setRequestModal(newRequestModel)
	}

	useEffect(() => {
		if (!show) {
			setRequestModal(emptyRequestModel)
		}
	}, [show])

	return (
		<Modal onClose={onClose} show={show} title={`Change Password for ${user?.firstName ?? ""} ${user?.lastName ?? ""}`}>
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Password</label>
						<div className="form-data">
							<Form.Input name="password" type="password" onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Confirm Password</label>
						<div className="form-data">
							<Form.Input name="passwordConfirmation" type="password" onChange={onChange} />
						</div>
					</div>
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} outline size="sm" onClick={onClose}>Cancel</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onUpdateUserPassword(requestModel)}>Save</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default UserUpdatePasswordModal
