import { serialize } from "object-to-formdata"
import ServiceBase from "asmr/services/ServiceBase"
import BeanResponseModel from "asmr/core/response/BeanResponseModel"
import BeansResponseModel from "asmr/core/response/BeansResponseModel"
import CreateBeanRequestModel from "asmr/core/request/CreateBeanRequestModel"
import UpdateBeanRequestModel from "asmr/core/request/UpdateBeanRequestModel"

class BeanService extends ServiceBase {
	public getAll() {
		return this.httpClient
			.get<BeansResponseModel>("/api/bean")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getById(id: string) {
		return this.httpClient
			.get<BeanResponseModel>(`/api/bean/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public create(body: CreateBeanRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.post<BeanResponseModel>("/api/bean", formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public modify(id: string, body: UpdateBeanRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.patch<BeanResponseModel>(`/api/bean/${id}`, formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public remove(id: string) {
		return this.httpClient
			.delete(`/api/bean/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default BeanService
