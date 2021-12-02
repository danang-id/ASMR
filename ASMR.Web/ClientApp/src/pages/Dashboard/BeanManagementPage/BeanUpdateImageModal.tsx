import { ChangeEvent, useEffect, useState } from "react"
import Button from "asmr/components/Button"
import Form from "asmr/components/Form"
import Modal from "asmr/components/Modal"
import Bean from "asmr/core/entities/Bean"
import UpdateBeanRequestModel from "asmr/core/request/UpdateBeanRequestModel"
import { ProgressInfo } from "asmr/libs/application/ProgressContextInfo"
import "asmr/pages/Dashboard/BeanManagementPage/BeansManagementModal.scoped.css"
import ImageCropper from "asmr/components/ImageCropper"
import { getFileFromCanvas } from "asmr/libs/common/canvas"

interface BeanUpdateImageModalProps {
	bean: Bean | null
	onClose: () => void
	onUpdateBean: (requestModel: UpdateBeanRequestModel, imageFile: File | null) => void
	progress: ProgressInfo
	show: boolean
}
function BeanUpdateImageModal({ bean, onClose, onUpdateBean, progress, show }: BeanUpdateImageModalProps): JSX.Element {
	const [croppedImageFile, setCroppedImageFile] = useState<File | null>(null)
	const [imageBuffer, setImageBuffer] = useState<string | ArrayBuffer | null>(null)
	const [imageFile, setImageFile] = useState<File | null>(null)

	function onChange(event: ChangeEvent<HTMLInputElement>) {
		if (event.target.name === "image" && event.target.files) {
			setImageFile(event.target.files[0])
		}
	}

	function onBeanImageCropped(cropper: Cropper) {
		const canvas = cropper.getCroppedCanvas()
		getFileFromCanvas(canvas, imageFile?.name ? imageFile.name : "", imageFile?.type).then(setCroppedImageFile)
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
		<Modal onClose={onClose} show={show} title={`Change Image for ${bean?.name ?? ""}`}>
			<Modal.Body>
				<Form className="modal-form">
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
							<ImageCropper alt={`${bean?.name ?? ""} Image`} aspectRatio={3/2} source={imageBuffer} onCropped={onBeanImageCropped} />
						</div>
					)}
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button
						disabled={progress.loading}
						size="sm"
						onClick={() => onUpdateBean({} as UpdateBeanRequestModel, croppedImageFile)}
					>
						Save
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default BeanUpdateImageModal
