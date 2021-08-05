import ServiceBase from "@asmr/services/ServiceBase"
import CreateProductRequestModel from "@asmr/data/request/CreateProductRequestModel"
import UpdateProductRequestModel from "@asmr/data/request/UpdateProductRequestModel"
import ProductResponseModel from "@asmr/data/response/ProductResponseModel"
import ProductsResponseModel from "@asmr/data/response/ProductsResponseModel"

class ProductService extends ServiceBase {
	public getAll() {
		return this.request<ProductsResponseModel>(() => (
			this.httpClient.get("/api/product")
		))
	}

	public getByBeanId(beanId: string) {
		return this.request<ProductsResponseModel>(() => (
			this.httpClient.get("/api/product", {
				params: { beanId }
			})
		))
	}

	public getById(id: string) {
		return this.request<ProductResponseModel>(() => (
			this.httpClient.get(`/api/product/${id}`)
		))
	}

	public create(body: CreateProductRequestModel) {
		return this.request<ProductResponseModel>(() => (
			this.httpClient.post("/api/product", body)
		))
	}

	public modify(id: string, body: UpdateProductRequestModel) {
		return this.request<ProductResponseModel>(() => (
			this.httpClient.patch(`/api/product/${id}`, body)
		))
	}

	public remove(id: string) {
		return this.request<ProductResponseModel>(() => (
			this.httpClient.delete(`/api/product/${id}`)
		))
	}
}

export default ProductService
