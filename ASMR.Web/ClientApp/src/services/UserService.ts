import { serialize } from "object-to-formdata"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import CreateUserRequestModel from "@asmr/data/request/CreateUserRequestModel"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import ApproveRegistrationRequestModel from "@asmr/data/request/ApproveRegistrationRequestModel"
import UserResponseModel from "@asmr/data/response/UserResponseModel"
import UsersResponseModel from "@asmr/data/response/UsersResponseModel"
import ServiceBase from "@asmr/services/ServiceBase"

class UserService extends ServiceBase {
	public getAll() {
		return this.httpClient
			.get<UsersResponseModel>("/api/user")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getById(id: string) {
		return this.httpClient
			.get<UserResponseModel>(`/api/user/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public create(body: CreateUserRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.post<UserResponseModel>("/api/user", formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public modify(id: string, body: UpdateUserRequestModel, imageFile: File | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.patch<UserResponseModel>(`/api/user/${id}`, formData)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public resetPassword(id: string) {
		return this.httpClient
			.post<DefaultResponseModel>(`/api/user/${id}/password-reset`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public remove(id: string) {
		return this.httpClient
			.delete<UserResponseModel>(`/api/user/${id}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public approve(id: string, body: ApproveRegistrationRequestModel) {
		return this.httpClient
			.post<UserResponseModel>(`/api/user/${id}/approve`, body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public reject(id: string) {
		return this.httpClient
			.post<UserResponseModel>(`/api/user/${id}/reject`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default UserService
