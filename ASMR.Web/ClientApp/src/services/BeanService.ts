import { serialize } from "object-to-formdata"
import ServiceBase from "asmr/services/ServiceBase"
import BeanResponseModel from "asmr/core/response/BeanResponseModel"
import BeansResponseModel from "asmr/core/response/BeansResponseModel"
import CreateBeanRequestModel from "asmr/core/request/CreateBeanRequestModel"
import UpdateBeanRequestModel from "asmr/core/request/UpdateBeanRequestModel"

class BeanService extends ServiceBase {
	private readonly path = "/api/Bean";

	public getAll() {
		return this.httpClient
			.get<BeansResponseModel>(this.path)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getById(id: string) {
		return this.httpClient
			.get<BeanResponseModel>(`${this.path}/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public create(body: CreateBeanRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.post<BeanResponseModel>(this.path, formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public modify(id: string, body: UpdateBeanRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.patch<BeanResponseModel>(`${this.path}/${id}`, formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public remove(id: string) {
		return this.httpClient
			.delete(`${this.path}/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default BeanService
