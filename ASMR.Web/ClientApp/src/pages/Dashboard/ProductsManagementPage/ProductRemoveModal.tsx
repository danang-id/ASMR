import Button from "@asmr/components/Button"
import Modal from "@asmr/components/Modal"
import Bean from "@asmr/data/models/Bean"
import Product from "@asmr/data/models/Product"
import { ProgressInfo } from "@asmr/libs/application/ProgressContextInfo"
import "@asmr/pages/Dashboard/ProductsManagementPage/ProductsManagementModal.scoped.css"
import { toLocalCurrency, toLocaleUnit } from "@asmr/libs/common/locale"

interface ProductRemoveModalProps {
	bean: Bean | null
	onClose: () => void
	onRemoveProduct: () => void
	product: Product | null
	progress: ProgressInfo
	show: boolean
}
function ProductRemoveModal({
	bean,
	onClose,
	onRemoveProduct,
	product,
	progress,
	show,
}: ProductRemoveModalProps): JSX.Element {
	return (
		<Modal onClose={onClose} show={show} title={`Remove ${bean?.name ?? ""}'s Product`}>
			<Modal.Body>
				<p className="remove-product-modal-body">
					Are you sure you would like to remove the <span className="name">{bean?.name ?? ""}</span> product{" "}
					{toLocaleUnit(product?.weightPerPackaging ? product.weightPerPackaging : 0, "gram")} /{" "}
					{toLocalCurrency(product?.price ? product.price : 0)}?
				</p>
			</Modal.Body>

			<Modal.Footer>
				<div className="modal-actions">
					<Button disabled={progress.loading} style="outline" size="sm" onClick={onClose}>
						Cancel
					</Button>
					<Button disabled={progress.loading} style="danger" size="sm" onClick={onRemoveProduct}>
						Remove
					</Button>
				</div>
			</Modal.Footer>
		</Modal>
	)
}

export default ProductRemoveModal
