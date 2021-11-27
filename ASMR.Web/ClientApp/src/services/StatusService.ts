import ServiceBase from "asmr/services/ServiceBase"
import ResponseModelBase from "asmr/core/common/ResponseModelBase"

class StatusService extends ServiceBase {
	public getStatus() {
		return this.httpClient
			.get<ResponseModelBase>("/api/status")
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default StatusService
