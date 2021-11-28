import ServiceBase from "asmr/services/ServiceBase"
import ResponseModelBase from "asmr/core/common/ResponseModelBase"

class StatusService extends ServiceBase {
	private readonly path = "/api/Status"

	public getStatus() {
		return this.httpClient
			.get<ResponseModelBase>(this.path)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default StatusService
