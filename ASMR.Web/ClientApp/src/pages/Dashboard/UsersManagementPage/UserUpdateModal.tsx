import { ChangeEvent, useEffect, useState } from "react"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import Modal from "@asmr/components/Modal"
import Role from "@asmr/data/enumerations/Role"
import User from "@asmr/data/models/User"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface UserUpdateModalProps {
	onClose: () => void
	onUpdateUser: (requestModel: UpdateUserRequestModel, imageFile: File | null) => void
	progress: ProgressInfo
	show: boolean
	user: User | null
}
function UserUpdateModal({ onClose, onUpdateUser, progress, show, user }: UserUpdateModalProps): JSX.Element {
	const emptyRequestModel: UpdateUserRequestModel = {
		firstName: "",
		lastName: "",
		emailAddress: "",
		username: "",
		roles: [],
	}
	const [isAdministrator, setIsAdministrator] = useState(false)
	const [requestModel, setRequestModal] = useState<UpdateUserRequestModel>(emptyRequestModel)

	function onChange(event: ChangeEvent<HTMLInputElement>) {
		const newRequestModel = { ...requestModel }
		if (event.target.name.startsWith("role")) {
			const roles: Role[] = [...newRequestModel.roles]
			const role: Role = parseInt(event.target.name.substring(5))
			const roleIndex = roles.indexOf(role)
			if (event.target.checked) {
				newRequestModel.roles = [...roles, role]
			} else if (roleIndex !== -1 && (role !== Role.Administrator || !isAdministrator)) {
				roles.splice(roleIndex, 1)
				newRequestModel.roles = [...roles]
			}
		} else {
			// @ts-ignore
			newRequestModel[event.target.name] = event.target.value
		}

		setRequestModal(newRequestModel)
	}

	function renderRolesAssignment(role: string, index: number): JSX.Element | null {
		if (typeof Role[Number(role)] !== "string" || Number(role) === Role.Administrator) {
			return null
		}

		const roles = requestModel.roles
		const checked = roles.includes(Number(role))
		const readOnly = Number(role) === Role.Administrator && roles.includes(Role.Administrator)
		return (
			<span key={index} className="role-checkbox">
				<Form.CheckBox checked={checked} name={`role-${role}`} readOnly={readOnly} onChange={onChange}>
					{Role[Number(role)]}
				</Form.CheckBox>
			</span>
		)
	}

	useEffect(() => {
		if (!show) {
			setIsAdministrator(false)
			setRequestModal(emptyRequestModel)
			return
		}
		if (user) {
			const newRequestModel: UpdateUserRequestModel = {
				firstName: user.firstName,
				lastName: user.lastName,
				emailAddress: user.emailAddress,
				username: user.username,
				roles: user.roles.map((userRole) => userRole.role),
			}
			setIsAdministrator(newRequestModel.roles.includes(Role.Administrator))
			setRequestModal(newRequestModel)
		}
	}, [show, user])

	return (
		<Modal onClose={onClose} show={show} title={`Profile: ${user?.firstName && ""} ${user?.lastName && ""}`}>
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">First Name</label>
						<div className="form-data">
							<Form.Input name="firstName" value={requestModel.firstName} onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Last Name</label>
						<div className="form-data">
							<Form.Input name="lastName" value={requestModel.lastName} onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Email Address</label>
						<div className="form-data">
							<Form.Input
								name="emailAddress"
								type="email"
								value={requestModel.emailAddress}
								onChange={onChange}
							/>
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Username</label>
						<div className="form-data">
							<Form.Input name="username" value={requestModel.username} onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Roles</label>
						{isAdministrator ? (
							<div className="form-data">
								<Form.Input disabled value={Role[Role.Administrator]} />
							</div>
						) : (
							<div className="form-data role-checkboxes">
								{Object.keys(Role).map(renderRolesAssignment)}
							</div>
						)}
					</div>
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onUpdateUser(requestModel, null)}>
						Save
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default UserUpdateModal
