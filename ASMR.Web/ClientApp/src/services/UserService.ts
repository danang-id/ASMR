import CreateUserRequestModel from "@asmr/data/request/CreateUserRequestModel"
import UpdateUserRequestModel from "@asmr/data/request/UpdateUserRequestModel"
import UpdateUserPasswordRequestModel from "@asmr/data/request/UpdateUserPasswordRequestModel"
import UserResponseModel from "@asmr/data/response/UserResponseModel"
import UsersResponseModel from "@asmr/data/response/UsersResponseModel"
import ServiceBase from "@asmr/services/ServiceBase"

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

	public modifyPassword(id: string, body: UpdateUserPasswordRequestModel) {
		return this.request<UserResponseModel>(() => (
			this.httpClient.patch(`/api/user/${id}/password`, body)
		))
	}

	public remove(id: string) {
		return this.request<UserResponseModel>(() => (
			this.httpClient.delete(`/api/user/${id}`)
		))
	}
}

export default UserService
