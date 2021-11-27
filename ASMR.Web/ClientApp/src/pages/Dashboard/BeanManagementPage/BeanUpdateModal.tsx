import { ChangeEvent, useEffect, useState } from "react"
import Button from "asmr/components/Button"
import Form from "asmr/components/Form"
import Modal from "asmr/components/Modal"
import Bean from "asmr/core/entities/Bean"
import UpdateBeanRequestModel from "asmr/core/request/UpdateBeanRequestModel"
import { ProgressInfo } from "asmr/libs/application/ProgressContextInfo"
import "asmr/pages/Dashboard/BeanManagementPage/BeansManagementModal.scoped.css"

interface BeanUpdateModalProps {
	bean: Bean | null
	onClose: () => void
	onUpdateBean: (requestModel: UpdateBeanRequestModel, imageFile: File | null) => void
	progress: ProgressInfo
	show: boolean
}
function BeanUpdateModal({ bean, onClose, onUpdateBean, progress, show }: BeanUpdateModalProps): JSX.Element {
	const emptyRequestModel: UpdateBeanRequestModel = {
		name: "",
		description: "",
	}
	const [requestModel, setRequestModal] = useState<UpdateBeanRequestModel>(emptyRequestModel)

	function onChange(event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) {
		const newRequestModel = { ...requestModel }
		// @ts-ignore
		newRequestModel[event.target.name] = event.target.value

		setRequestModal(newRequestModel)
	}

	useEffect(() => {
		if (!show) {
			setRequestModal(emptyRequestModel)
			return
		}
		if (bean) {
			const newRequestModel: UpdateBeanRequestModel = {
				name: bean.name,
				description: bean.description,
			}
			setRequestModal(newRequestModel)
		}
	}, [show, bean])

	return (
		<Modal onClose={onClose} show={show} title={`Modify ${bean?.name ?? ""}`}>
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Name</label>
						<div className="form-data">
							<Form.Input name="name" value={requestModel.name} onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Description</label>
						<div className="form-data">
							<Form.TextArea
								name="description"
								rows={6}
								value={requestModel.description}
								onChange={onChange}
							/>
						</div>
					</div>
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onUpdateBean(requestModel, null)}>
						Save
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default BeanUpdateModal
