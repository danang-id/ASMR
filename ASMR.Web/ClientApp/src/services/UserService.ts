import CreateUserRequestModel from "@asmr/data/request/CreateUserRequestModel"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import ApproveRegistrationRequestModel from "@asmr/data/request/ApproveRegistrationRequestModel"
import UserResponseModel from "@asmr/data/response/UserResponseModel"
import UsersResponseModel from "@asmr/data/response/UsersResponseModel"
import ServiceBase from "@asmr/services/ServiceBase"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"

class UserService extends ServiceBase {
	public getAll() {
		return this.request<UsersResponseModel>(() => (
			this.httpClient.get("/api/user")
		))
	}

	public getById(id: string) {
		return this.request<UserResponseModel>(() => (
			this.httpClient.get(`/api/user/${id}`)
		))
	}

	public create(body: CreateUserRequestModel, imageFile: File | null) {
		const formData = this.createFormData(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.request<UserResponseModel>(() => (
			this.httpClient.post("/api/user", formData)
		))
	}

	public modify(id: string, body: UpdateUserRequestModel, imageFile: File | null) {
		const formData = this.createFormData(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.request<UserResponseModel>(() => (
			this.httpClient.patch(`/api/user/${id}`, formData)
		))
	}

	public resetPassword(id: string) {
		return this.request<DefaultResponseModel<undefined>>(() => (
			this.httpClient.post(`/api/user/${id}/password-reset`)
		))
	}

	public remove(id: string) {
		return this.request<UserResponseModel>(() => (
			this.httpClient.delete(`/api/user/${id}`)
		))
	}

	public approve(id: string, body: ApproveRegistrationRequestModel) {
		return this.request<UserResponseModel>(() => (
			this.httpClient.post(`/api/user/${id}/approve`, body)
		))
	}

	public reject(id: string) {
		return this.request<UserResponseModel>(() => (
			this.httpClient.post(`/api/user/${id}/reject`)
		))
	}
}

export default UserService
