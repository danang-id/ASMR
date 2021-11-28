import ServiceBase from "asmr/services/ServiceBase"
import ResponseModelBase from "asmr/core/common/ResponseModelBase"
import ASMRMobileReleaseInformation from "asmr/core/release/ASMRMobileReleaseInformation"
import ASMRWebReleaseInformation from "asmr/core/release/ASMRWebReleaseInformation"

class ReleaseService extends ServiceBase {
	private readonly path = "/api/Release"

	public getMobileReleaseInformation() {
		return this.httpClient
			.get<ResponseModelBase<ASMRMobileReleaseInformation>>(`${this.path}/mobile`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getWebReleaseInformation() {
		return this.httpClient
			.get<ResponseModelBase<ASMRWebReleaseInformation>>(`${this.path}/web`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default ReleaseService
