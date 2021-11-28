
import BackButton from "asmr/components/BackButton"
import Bean from "asmr/core/entities/Bean"
import DashboardLayout from "asmr/layouts/DashboardLayout"
import useInit from "asmr/libs/hooks/initHook"
import useLogger from "asmr/libs/hooks/loggerHook"
import useServices from "asmr/libs/hooks/servicesHook"
import "asmr/pages/Dashboard/ProductionReportPage/ProductionReportPage.scoped.css"

function ProductionReportPage(): JSX.Element {
	useInit(onInit)
	const logger = useLogger(ProductionReportPage)
	const services = useServices()

	async function onInit() {
		const beans = await retrieveAllBeans()
		for (const bean of beans) {
			await retrieveBusinessAnalyticsByBean(bean)
		}
	}

	async function retrieveAllBeans() {
		try {
			const result = await services.bean.getAll()
			if (result.isSuccess && result.data) {
				return result.data
			}

			services.handleErrors(result.errors, void 0, logger)
		} catch (error) {
			services.handleError(error as Error, void 0, logger)
		}

		return []
	}

	async function retrieveBusinessAnalyticsByBean(bean: Bean) {
		try {
			const result = await services.businessAnalytic.getByBeanId(bean.id)
			if (result.isSuccess && result.data) {
				logger.info(bean.name, result.data)
			}

			services.handleErrors(result.errors, void 0, logger)
		} catch (error) {
			services.handleError(error as Error, void 0, logger)
		}
	}

	return (
		<DashboardLayout>
			<div className="header">
				<BackButton />
				&nbsp;&nbsp; Production Report
			</div>
			<div className="content">
				<div>
					<div className="warning-box">
						We are sorry, this feature is not currently available at the moment.
						<br />
						Please try again later.
					</div>
				</div>
			</div>
		</DashboardLayout>
	)
}

export default ProductionReportPage
