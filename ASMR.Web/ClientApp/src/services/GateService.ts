
import RegistrationRequestModel from "@asmr/data/request/RegistrationRequestModel"
import ResendEmailAddressConfirmationRequestModel from "@asmr/data/request/ResendEmailAddressConfirmationRequestModel"
import ConfirmEmailAddressRequestModel from "@asmr/data/request/ConfirmEmailAddressRequestModel"
import SignInRequestModel from "@asmr/data/request/SignInRequestModel"
import ForgetPasswordRequestModel from "@asmr/data/request/ForgetPasswordRequestModel"
import ResetPasswordRequestModel from "@asmr/data/request/ResetPasswordRequestModel"
import AuthenticationResponseModel from "@asmr/data/response/AuthenticationResponseModel"
import ServiceBase from "@asmr/services/ServiceBase"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"

class GateService extends ServiceBase {
	public register(body: RegistrationRequestModel, imageFile: File | null, captchaResponseToken: string | null) {
		const formData = this.createFormData(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.request<DefaultResponseModel<undefined>>(() => (
			this.httpClient.post("/api/gate/register", formData, {
				params: { captchaResponseToken }
			})
		))
	}

	public resendEmailAddressConfirmation(body: ResendEmailAddressConfirmationRequestModel) {
		return this.request<DefaultResponseModel<undefined>>(() => (
			this.httpClient.post("/api/gate/email-address/resend-confirmation", body)
		))
	}

	public confirmEmailAddress(body: ConfirmEmailAddressRequestModel) {
		return this.request<DefaultResponseModel<undefined>>(() => (
			this.httpClient.post("/api/gate/email-address/confirm", body)
		))
	}

	public authenticate(body: SignInRequestModel) {
		return this.request<AuthenticationResponseModel>(() => (
			this.httpClient.post("/api/gate/authenticate", body)
		))
	}

	public clearSession() {
		return this.request<AuthenticationResponseModel>(() => (
			this.httpClient.post("/api/gate/exit")
		))
	}

	public forgetPassword(body: ForgetPasswordRequestModel, captchaResponseToken: string | null) {
		return this.request<DefaultResponseModel<undefined>>(() => (
			this.httpClient.post("/api/gate/password/forget", body, {
				params: { captchaResponseToken }
			})
		))
	}

	public resetPassword(body: ResetPasswordRequestModel) {
		return this.request<DefaultResponseModel<undefined>>(() => (
			this.httpClient.post("/api/gate/password/reset", body)
		))
	}

	public getUserPassport() {
		return this.request<AuthenticationResponseModel>(() => (
			this.httpClient.get("/api/gate/passport")
		))
	}
}

export default GateService
