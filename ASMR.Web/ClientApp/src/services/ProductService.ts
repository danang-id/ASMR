import ServiceBase from "asmr/services/ServiceBase"
import CreateProductRequestModel from "asmr/core/request/CreateProductRequestModel"
import UpdateProductRequestModel from "asmr/core/request/UpdateProductRequestModel"
import ProductResponseModel from "asmr/core/response/ProductResponseModel"
import ProductsResponseModel from "asmr/core/response/ProductsResponseModel"

class ProductService extends ServiceBase {
	public getAll() {
		return this.httpClient
			.get<ProductsResponseModel>("/api/product")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getByBeanId(beanId: string) {
		return this.httpClient
			.get<ProductsResponseModel>("/api/product", {
				params: { beanId },
			})
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getById(id: string) {
		return this.httpClient
			.get<ProductResponseModel>(`/api/product/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public create(body: CreateProductRequestModel) {
		return this.httpClient
			.post<ProductResponseModel>("/api/product", body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public modify(id: string, body: UpdateProductRequestModel) {
		return this.httpClient
			.patch<ProductResponseModel>(`/api/product/${id}`, body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public remove(id: string) {
		return this.httpClient
			.delete<ProductResponseModel>(`/api/product/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default ProductService
