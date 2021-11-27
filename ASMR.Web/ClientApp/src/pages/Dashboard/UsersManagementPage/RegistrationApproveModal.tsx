import { ChangeEvent, useEffect, useState } from "react"
import Button from "asmr/components/Button"
import Form from "asmr/components/Form"
import Modal from "asmr/components/Modal"
import User from "asmr/core/entities/User"
import { ProgressInfo } from "asmr/libs/application/ProgressContextInfo"
import Role from "asmr/core/enums/Role"
import ApproveRegistrationRequestModel from "asmr/core/request/ApproveRegistrationRequestModel"
import "asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface RegistrationApproveModalProps {
	onClose: () => void
	onApproveRegistration: (requestModel: ApproveRegistrationRequestModel) => void
	progress: ProgressInfo
	show: boolean
	user: User | null
}
function RegistrationApproveModal({
	onClose,
	onApproveRegistration,
	progress,
	show,
	user,
}: RegistrationApproveModalProps): JSX.Element {
	const emptyRequestModel: ApproveRegistrationRequestModel = {
		roles: [],
	}
	const [requestModel, setRequestModal] = useState<ApproveRegistrationRequestModel>(emptyRequestModel)

	function onChange(event: ChangeEvent<HTMLInputElement>) {
		const newRequestModel = { ...requestModel }
		if (event.target.name.startsWith("role")) {
			const roles: Role[] = newRequestModel.roles ?? []
			const role: Role = parseInt(event.target.name.substring(5))
			const roleIndex = roles.indexOf(role)
			if (event.target.checked) {
				newRequestModel.roles = [...roles, role]
			} else if (roleIndex !== -1) {
				roles.splice(roleIndex, 1)
				newRequestModel.roles = [...roles, roleIndex]
			}
		}

		setRequestModal(newRequestModel)
	}

	function renderRolesAssignment(role: string, index: number): JSX.Element | null {
		if (typeof Role[Number(role)] !== "string" || Number(role) === Role.Administrator) {
			return null
		}

		return (
			<span key={index} className="role-checkbox">
				<Form.CheckBox name={`role-${role}`} onChange={onChange}>
					{Role[Number(role)]}
				</Form.CheckBox>
			</span>
		)
	}

	useEffect(() => {
		if (!show) {
			setRequestModal(emptyRequestModel)
		}
	}, [show])

	return (
		<Modal
			onClose={onClose}
			show={show}
			title={`Approve Registration of ${user?.firstName ?? ""} ${user?.lastName ?? ""}`}
		>
			<Modal.Body>
				<div className="approve-registration-modal-body">
					<p>
						To approve the registration of user{" "}
						<span className="full-name">
							{user?.firstName} {user?.lastName}
						</span>
						, please assign roles for this user.
					</p>
				</div>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Roles</label>
						<div className="form-data role-checkboxes">{Object.keys(Role).map(renderRolesAssignment)}</div>
					</div>
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onApproveRegistration(requestModel)}>
						Approve
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default RegistrationApproveModal
