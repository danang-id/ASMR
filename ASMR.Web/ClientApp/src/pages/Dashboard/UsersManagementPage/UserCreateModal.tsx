
import { ChangeEvent, useEffect, useState } from "react"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import Modal from "@asmr/components/Modal"
import Role from "@asmr/data/enumerations/Role"
import CreateUserRequestModel from "@asmr/data/request/CreateUserRequestModel"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"
import ImageCropper from "@asmr/components/ImageCropper"
import { getFileFromCanvas } from "@asmr/libs/common/canvas"

interface UserCreateModalProps {
	onClose: () => void
	onCreateUser: (requestModel: CreateUserRequestModel, imageFile: File | null) => void
	progress: ProgressInfo,
	show: boolean
}
function UserCreateModal({ onClose, onCreateUser, progress, show  }: UserCreateModalProps): JSX.Element {
	const emptyRequestModel: CreateUserRequestModel = {
		firstName: "",
		lastName: "",
		emailAddress: "",
		username: "",
		password: "",
		passwordConfirmation: "",
		roles: []
	}
	const [croppedImageFile, setCroppedImageFile] = useState<File | null>(null)
	const [imageBuffer, setImageBuffer] = useState<string | ArrayBuffer | null>(null)
	const [imageFile, setImageFile] = useState<File | null>(null)
	const [requestModel, setRequestModal] = useState<CreateUserRequestModel>(emptyRequestModel)

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
		} if (event.target.name === "image" && event.target.files) {
			setImageFile(event.target.files[0])
		} else {
			// @ts-ignore
			newRequestModel[event.target.name] = event.target.value
		}

		setRequestModal(newRequestModel)
	}

	function onUserImageCropped(cropper: Cropper) {
		const canvas = cropper.getCroppedCanvas()
		getFileFromCanvas(canvas, imageFile?.name ?? "", imageFile?.type)
			.then(setCroppedImageFile)
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
			setImageBuffer(null)
			setRequestModal(emptyRequestModel)
		}
	}, [show])

	useEffect(() => {
		if (!imageFile) {
			setImageBuffer(null)
			return
		}

		const fileReader = new FileReader()
		fileReader.onloadend = () => {
			setImageBuffer(fileReader.result)
		}
		fileReader.readAsDataURL(imageFile)
	}, [imageFile])

	return (
		<Modal onClose={onClose} show={show} title="Create New User">
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">First Name</label>
						<div className="form-data">
							<Form.Input name="firstName" onChange={onChange}
							/>
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Last Name</label>
						<div className="form-data">
							<Form.Input name="lastName" onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Email Address</label>
						<div className="form-data">
							<Form.Input name="emailAddress" type="email" onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Username</label>
						<div className="form-data">
							<Form.Input name="username" onChange={onChange} />
						</div>
					</div>
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
					<div className="form-row">
						<label className="form-field">Roles</label>
						<div className="form-data role-checkboxes">
							{
								Object.keys(Role).map(renderRolesAssignment)
							}
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Image</label>
						<div className="form-data">
							<Form.Input accept="image/png, image/jpeg, image/webp" name="image" type="file" onChange={onChange} />
						</div>
					</div>
					{
						imageBuffer && (
							<div className="form-row image-preview">
								<ImageCropper alt="User Image" source={imageBuffer} onCropped={onUserImageCropped} />
							</div>
						)
					}
				</Form>
			</Modal.Body>

			<Modal.Footer className="modal-actions">
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onCreateUser(requestModel, croppedImageFile)}>
						Create
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default UserCreateModal
