import ServiceBase from "@asmr/services/ServiceBase"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import ASMRMobileReleaseInformation from "@asmr/data/release/ASMRMobileReleaseInformation"
import ASMRWebReleaseInformation from "@asmr/data/release/ASMRWebReleaseInformation"

class ReleaseService extends ServiceBase {
	public getMobileReleaseInformation() {
		return this.request<DefaultResponseModel<ASMRMobileReleaseInformation>>(() => (
			this.httpClient.get("/api/release/mobile")
		))
	}
	
	public getWebReleaseInformation() {
		return this.request<DefaultResponseModel<ASMRWebReleaseInformation>>(() => (
			this.httpClient.get("/api/release/web")
		))
	}
}

export default ReleaseService
