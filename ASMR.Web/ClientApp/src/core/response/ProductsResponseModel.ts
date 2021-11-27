import ResponseModelBase from "asmr/core/common/ResponseModelBase"
import Product from "asmr/core/entities/Product"

interface ProductsResponseModel extends ResponseModelBase<Product[]> {}

export default ProductsResponseModel
