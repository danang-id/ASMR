
import { ChangeEvent, useEffect, useState } from "react"
import Button from "@asmr/components/Button"
import Form from "@asmr/components/Form"
import Modal from "@asmr/components/Modal"
import Bean from "@asmr/data/models/Bean"
import Product  from "@asmr/data/models/Product"
import UpdateProductRequestModel from "@asmr/data/request/UpdateProductRequestModel"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/ProductsManagementPage/ProductsManagementModal.scoped.css"

interface ProductUpdateModalProps {
	bean: Bean | null,
	onClose: () => void
	onUpdateProduct: (requestModel: UpdateProductRequestModel) => void
	product: Product | null,
	progress: ProgressInfo,
	show: boolean
}
function ProductUpdateModal({ bean, product, onClose, onUpdateProduct, progress, show }: ProductUpdateModalProps): JSX.Element {
	const emptyRequestModel: UpdateProductRequestModel = {
		price: 0,
		weightPerPackaging: 0
	}
	const [requestModel, setRequestModal] = useState<UpdateProductRequestModel>(emptyRequestModel)

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
		if (product) {
			const newRequestModel: UpdateProductRequestModel = {
				price: product.price,
				weightPerPackaging: product.weightPerPackaging,
			}
			setRequestModal(newRequestModel)
		}
	}, [show, product])

	return (
		<Modal onClose={onClose} show={show} title={`Modify ${bean?.name ?? ""}'s Product`}>
			<Modal.Body>
				<Form className="modal-form">
					<div className="form-row">
						<label className="form-field">Weight per Packaging (gram)</label>
						<div className="form-data">
							<Form.Input name="weightPerPackaging" type="number" value={requestModel.weightPerPackaging as any} onChange={onChange} />
						</div>
					</div>
					<div className="form-row">
						<label className="form-field">Price (Rp)</label>
						<div className="form-data">
							<Form.Input name="price" type="number" value={requestModel.price as any} onChange={onChange} />
						</div>
					</div>
				</Form>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} size="sm" onClick={() => onUpdateProduct(requestModel)}>
						Save
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default ProductUpdateModal
