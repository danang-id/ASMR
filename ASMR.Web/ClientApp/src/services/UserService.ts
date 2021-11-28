import { serialize } from "object-to-formdata"
import ResponseModelBase from "asmr/core/common/ResponseModelBase"
import CreateUserRequestModel from "asmr/core/request/CreateUserRequestModel"
import UpdateUserRequestModel from "asmr/core/request/UpdateUserRequestModel"
import ApproveRegistrationRequestModel from "asmr/core/request/ApproveRegistrationRequestModel"
import UserResponseModel from "asmr/core/response/UserResponseModel"
import UsersResponseModel from "asmr/core/response/UsersResponseModel"
import ServiceBase from "asmr/services/ServiceBase"

class UserService extends ServiceBase {
	private readonly path = "/api/User"

	public getAll() {
		return this.httpClient
			.get<UsersResponseModel>(this.path)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getById(id: string) {
		return this.httpClient
			.get<UserResponseModel>(`${this.path}/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public create(body: CreateUserRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.post<UserResponseModel>(this.path, formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public modify(id: string, body: UpdateUserRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.patch<UserResponseModel>(`${this.path}/${id}`, formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public resetPassword(id: string) {
		return this.httpClient
			.post<ResponseModelBase>(`${this.path}/${id}/password-reset`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public remove(id: string) {
		return this.httpClient
			.delete<UserResponseModel>(`${this.path}/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public approve(id: string, body: ApproveRegistrationRequestModel) {
		return this.httpClient
			.post<UserResponseModel>(`${this.path}/${id}/approve`, body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public reject(id: string) {
		return this.httpClient
			.post<UserResponseModel>(`${this.path}/${id}/reject`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default UserService
