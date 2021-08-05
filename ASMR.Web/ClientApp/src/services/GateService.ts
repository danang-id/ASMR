import SignInRequestModel from "@asmr/data/request/SignInRequestModel"
import AuthenticationResponseModel from "@asmr/data/response/AuthenticationResponseModel"
import ServiceBase from "@asmr/services/ServiceBase"

class GateService extends ServiceBase {
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

	public getUserPassport() {
		return this.request<AuthenticationResponseModel>(() => (
			this.httpClient.get("/api/gate/passport")
		))
	}
}

export default GateService
