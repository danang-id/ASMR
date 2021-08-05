
import { ChangeEvent, useEffect, useState } from "react"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import Modal from "@asmr/components/Modal"
import Bean from "@asmr/data/models/Bean"
import CreateProductRequestModel from "@asmr/data/request/CreateProductRequestModel"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/ProductsManagementPage/ProductsManagementModal.scoped.css"

interface ProductCreateModalProps {
	bean: Bean | null
	onClose: () => void
	onCreateProduct: (requestModel: CreateProductRequestModel) => void
	progress: ProgressInfo,
	show: boolean
}
function ProductCreateModal({ bean, onClose, onCreateProduct, progress, show  }: ProductCreateModalProps): JSX.Element {
	const emptyRequestModel: CreateProductRequestModel = {
		beanId: bean?.id ?? "",
		price: 0,
		weightPerPackaging: 0
	}
	const [requestModel, setRequestModal] = useState<CreateProductRequestModel>(emptyRequestModel)

	function onChange(event: ChangeEvent<HTMLInputElement>) {
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
			setRequestModal({
				beanId: bean.id,
				price: 0,
				weightPerPackaging: 0
			})
		}
	}, [show, bean])

	return (
		<Modal onClose={onClose} show={show} title={`Add Product for ${bean?.name ?? ""}`}>
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Weight per Packaging (gram)</label>
						<div className="form-data">
							<Form.Input name="weightPerPackaging" type="number" onChange={onChange}
							/>
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Price (Rp)</label>
						<div className="form-data">
							<Form.Input name="price" type="number" onChange={onChange}
							/>
						</div>
					</div>
				</Form>
			</Modal.Body>

			<Modal.Footer className="modal-actions">
				<div className="modal-actions">
					<Button disabled={progress.loading} outline size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onCreateProduct(requestModel)}>
						Create
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default ProductCreateModal
