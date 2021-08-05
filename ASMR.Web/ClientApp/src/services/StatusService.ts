import ServiceBase from "@asmr/services/ServiceBase"
import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"

class StatusService extends ServiceBase {
	public getStatus() {
		return this.request<DefaultResponseModel>(() => (
			this.httpClient.get("/api/status")
		))
	}
}

export default StatusService
