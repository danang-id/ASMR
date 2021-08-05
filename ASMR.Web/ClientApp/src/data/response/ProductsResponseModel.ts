import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import Product from "@asmr/data/models/Product"

interface ProductsResponseModel extends DefaultResponseModel<Product[]> {}

export default ProductsResponseModel
