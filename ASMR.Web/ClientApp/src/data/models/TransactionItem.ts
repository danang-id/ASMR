import DefaultModel from "@asmr/data/generic/DefaultModel"
import Product from "@asmr/data/models/Product"

interface TransactionItem extends DefaultModel {
	transactionId: string
	product: Product
	quantity: number
	price: number
}

export default TransactionItem
