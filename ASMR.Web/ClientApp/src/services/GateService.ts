import { serialize } from "object-to-formdata"
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
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.post<DefaultResponseModel>("/api/gate/register", formData, {
				params: { captchaResponseToken }
			})
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public resendEmailAddressConfirmation(body: ResendEmailAddressConfirmationRequestModel) {
		return this.httpClient
			.post<DefaultResponseModel>("/api/gate/email-address/resend-confirmation", body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public confirmEmailAddress(body: ConfirmEmailAddressRequestModel) {
		return this.httpClient
			.post<DefaultResponseModel>("/api/gate/email-address/confirm", body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public authenticate(body: SignInRequestModel) {
		return this.httpClient
			.post<AuthenticationResponseModel>("/api/gate/authenticate", body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public clearSession() {
		return this.httpClient
			.post<AuthenticationResponseModel>("/api/gate/exit")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public forgetPassword(body: ForgetPasswordRequestModel, captchaResponseToken: string | null) {
		return this.httpClient
			.post<DefaultResponseModel>("/api/gate/password/forget", body, {
				params: { captchaResponseToken }
			})
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public resetPassword(body: ResetPasswordRequestModel) {
		return this.httpClient
			.post<DefaultResponseModel>("/api/gate/password/reset", body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getUserPassport() {
		return this.httpClient
			.get<AuthenticationResponseModel>("/api/gate/passport")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default GateService
