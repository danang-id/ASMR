import { serialize } from "object-to-formdata"
import RegistrationRequestModel from "asmr/core/request/RegistrationRequestModel"
import ResendEmailAddressConfirmationRequestModel from "asmr/core/request/ResendEmailAddressConfirmationRequestModel"
import ConfirmEmailAddressRequestModel from "asmr/core/request/ConfirmEmailAddressRequestModel"
import SignInRequestModel from "asmr/core/request/SignInRequestModel"
import ForgetPasswordRequestModel from "asmr/core/request/ForgetPasswordRequestModel"
import ResetPasswordRequestModel from "asmr/core/request/ResetPasswordRequestModel"
import AuthenticationResponseModel from "asmr/core/response/AuthenticationResponseModel"
import ServiceBase from "asmr/services/ServiceBase"
import ResponseModelBase from "asmr/core/common/ResponseModelBase"

class GateService extends ServiceBase {
	public register(body: RegistrationRequestModel, imageFile: File | null, captchaResponseToken: string | null) {
		const formData = serialize(body)
		if (imageFile) {
			formData.append("image", imageFile, imageFile.name)
		}
		return this.httpClient
			.post<ResponseModelBase>("/api/gate/register", formData, {
				params: { captchaResponseToken },
			})
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public resendEmailAddressConfirmation(body: ResendEmailAddressConfirmationRequestModel) {
		return this.httpClient
			.post<ResponseModelBase>("/api/gate/email-address/resend-confirmation", body)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public confirmEmailAddress(body: ConfirmEmailAddressRequestModel) {
		return this.httpClient
			.post<ResponseModelBase>("/api/gate/email-address/confirm", body)
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
			.post<ResponseModelBase>("/api/gate/password/forget", body, {
				params: { captchaResponseToken },
			})
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public resetPassword(body: ResetPasswordRequestModel) {
		return this.httpClient
			.post<ResponseModelBase>("/api/gate/password/reset", body)
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
