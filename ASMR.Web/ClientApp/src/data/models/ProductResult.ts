import DefaultModel from "@asmr/data/generic/DefaultModel"
import Product from "@asmr/data/models/Product"

interface ProductResult extends DefaultModel {
	product: Product
	weight: number
}

export default ProductResult
