import ServiceBase from "@asmr/services/ServiceBase"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"

class StatusService extends ServiceBase {
	public getStatus() {
		return this.httpClient
			.get<DefaultResponseModel>("/api/status")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default StatusService
