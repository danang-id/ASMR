import DefaultModel from "@asmr/data/generic/DefaultModel"

interface Product extends DefaultModel {
	beanId: string
	currentInventoryQuantity: number
	price: number
	weightPerPackaging: number
}
export default Product
