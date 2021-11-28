import ServiceBase from "asmr/services/ServiceBase"
import BusinessAnalyticsResponseModel from "asmr/core/response/BusinessAnalyticsResponseModel"

class BusinessAnalyticService extends ServiceBase {
	private readonly path = "/api/BusinessAnalytic"

	public getMine() {
		return this.httpClient
			.get<BusinessAnalyticsResponseModel>(this.path)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getByBeanId(beanId: string) {
		return this.httpClient
			.get<BusinessAnalyticsResponseModel>(`${this.path}/bean/${beanId}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}

	public getByUserId(userId: string) {
		return this.httpClient
			.get<BusinessAnalyticsResponseModel>(`${this.path}/user/${userId}`)
			.then(this.processResponse.bind(this))
			.finally(this.finalize.bind(this))
	}
}

export default BusinessAnalyticService
