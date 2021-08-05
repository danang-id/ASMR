import ServiceBase from "@asmr/services/ServiceBase"
import BeanResponseModel from "@asmr/data/response/BeanResponseModel"
import BeansResponseModel from "@asmr/data/response/BeansResponseModel"
import CreateBeanRequestModel from "@asmr/data/request/CreateBeanRequestModel"
import UpdateBeanRequestModel from "@asmr/data/request/UpdateBeanRequestModel"

class BeanService extends ServiceBase {
	public getAll() {
		return this.request<BeansResponseModel>(() => (
			this.httpClient.get("/api/bean")
		))
	}

	public getById(id: string) {
		return this.request<BeanResponseModel>(() => (
			this.httpClient.get(`/api/bean/${id}`)
		))
	}

	public create(body: CreateBeanRequestModel, imageFile: File | null) {
		const formData = this.createFormData(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.request<BeanResponseModel>(() => (
			this.httpClient.post("/api/bean", formData)
		))
	}

	public modify(id: string, body: UpdateBeanRequestModel, imageFile: File | null) {
		const formData = this.createFormData(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.request<BeanResponseModel>(() => (
			this.httpClient.patch(`/api/bean/${id}`, formData)
		))
	}

	public remove(id: string) {
		return this.request<BeanResponseModel>(() => (
			this.httpClient.delete(`/api/bean/${id}`)
		))
	}
}

export default BeanService
