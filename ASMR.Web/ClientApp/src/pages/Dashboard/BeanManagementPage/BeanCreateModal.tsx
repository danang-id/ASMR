import { ChangeEvent, useEffect, useState } from "react"
import Button from "asmr/components/Button"
import Form from "asmr/components/Form"
import Modal from "asmr/components/Modal"
import CreateBeanRequestModel from "asmr/core/request/CreateBeanRequestModel"
import { ProgressInfo } from "asmr/libs/application/ProgressContextInfo"
import "asmr/pages/Dashboard/BeanManagementPage/BeansManagementModal.scoped.css"
import ImageCropper from "asmr/components/ImageCropper"
import { getFileFromCanvas } from "asmr/libs/common/canvas"

interface BeanCreateModalProps {
	onClose: () => void
	onCreateBean: (requestModel: CreateBeanRequestModel, imageFile: File | null) => void
	progress: ProgressInfo
	show: boolean
}
function BeanCreateModal({ onClose, onCreateBean, progress, show }: BeanCreateModalProps): JSX.Element {
	const emptyRequestModel: CreateBeanRequestModel = {
		name: "",
		description: "",
	}
	const [croppedImageFile, setCroppedImageFile] = useState<File | null>(null)
	const [imageBuffer, setImageBuffer] = useState<string | ArrayBuffer | null>(null)
	const [imageFile, setImageFile] = useState<File | null>(null)
	const [requestModel, setRequestModal] = useState<CreateBeanRequestModel>(emptyRequestModel)

	function onChange(event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) {
		const newRequestModel = { ...requestModel }

		const inputElementEvent = event as ChangeEvent<HTMLInputElement>
		if (event.target.name === "image" && inputElementEvent.target.files) {
			setImageFile(inputElementEvent.target.files[0])
		} else {
			// @ts-ignore
			newRequestModel[event.target.name] = event.target.value
			setRequestModal(newRequestModel)
		}
	}

	function onBeanImageCropped(cropper: Cropper) {
		const canvas = cropper.getCroppedCanvas()
		getFileFromCanvas(canvas, imageFile?.name ? imageFile.name : "", imageFile?.type).then(setCroppedImageFile)
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
		<Modal onClose={onClose} show={show} title="Add Bean">
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Name</label>
						<div className="form-data">
							<Form.Input name="name" onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Description</label>
						<div className="form-data">
							<Form.TextArea name="description" rows={6} onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Image</label>
						<div className="form-data">
							<Form.Input
								accept="image/png, image/jpeg, image/webp"
								name="image"
								type="file"
								onChange={onChange}
							/>
						</div>
					</div>
					{imageBuffer && (
						<div className="form-row image-preview">
							<ImageCropper alt="Bean Image" aspectRatio={3/2} source={imageBuffer} onCropped={onBeanImageCropped} />
						</div>
					)}
				</Form>
			</Modal.Body>

			<Modal.Footer className="modal-actions">
				<div className="modal-actions">
					<Button disabled={progress.loading} size="sm" style="outline" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onCreateBean(requestModel, croppedImageFile)}>
						Create
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default BeanCreateModal
