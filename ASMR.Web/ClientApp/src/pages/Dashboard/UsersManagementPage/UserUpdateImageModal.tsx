
import { ChangeEvent, useEffect, useState } from "react"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import ImageCropper from "@asmr/components/ImageCropper"
import Modal from "@asmr/components/Modal"
import User from "@asmr/data/models/User"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import { getFileFromCanvas } from "@asmr/libs/common/canvas"
import "@asmr/pages/Dashboard/UsersManagementPage/UsersManagementModal.scoped.css"

interface UserUpdateImageModalProps {
	onClose: () => void
	onUpdateUser: (requestModel: UpdateUserRequestModel, imageFile: File | null) => void
	progress: ProgressInfo,
	show: boolean
	user: User | null
}
function UserUpdateImageModal({ user, onClose, onUpdateUser, progress, show }: UserUpdateImageModalProps): JSX.Element {
	const [croppedImageFile, setCroppedImageFile] = useState<File | null>(null)
	const [imageBuffer, setImageBuffer] = useState<string | ArrayBuffer | null>(null)
	const [imageFile, setImageFile] = useState<File | null>(null)

	function onChange(event: ChangeEvent<HTMLInputElement>) {
		if (event.target.name === "image" && event.target.files) {
			setImageFile(event.target.files[0])
		}
	}

	function onUserImageCropped(cropper: Cropper) {
		const canvas = cropper.getCroppedCanvas()
		getFileFromCanvas(canvas, imageFile?.name ?? "", imageFile?.type)
			.then(setCroppedImageFile)
	}

	useEffect(() => {
		if (!show) {
			setImageBuffer(null)
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
		<Modal onClose={onClose} show={show} title={`Change Image for ${user?.firstName ?? ""} ${user?.lastName ?? ""}`}>
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Image</label>
						<div className="form-data">
							<Form.Input accept="image/png, image/jpeg, image/webp" name="image" type="file" onChange={onChange} />
						</div>
					</div>
					{
						imageBuffer && (
							<div className="form-row image-preview">
								<ImageCropper alt={`${user?.firstName ?? ""} Image`}  source={imageBuffer}
										onCropped={onUserImageCropped} />
							</div>
						)
					}
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} outline size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm"
							onClick={() => onUpdateUser({} as UpdateUserRequestModel, croppedImageFile)}>
						Save
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default UserUpdateImageModal
