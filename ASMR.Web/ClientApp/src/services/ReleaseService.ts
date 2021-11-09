import ServiceBase from "@asmr/services/ServiceBase"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import ASMRMobileReleaseInformation from "@asmr/data/release/ASMRMobileReleaseInformation"
import ASMRWebReleaseInformation from "@asmr/data/release/ASMRWebReleaseInformation"

class ReleaseService extends ServiceBase {
	public getMobileReleaseInformation() {
		return this.httpClient
			.get<DefaultResponseModel<ASMRMobileReleaseInformation>>("/api/release/mobile")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getWebReleaseInformation() {
		return this.httpClient
			.get<DefaultResponseModel<ASMRWebReleaseInformation>>("/api/release/web")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default ReleaseService
