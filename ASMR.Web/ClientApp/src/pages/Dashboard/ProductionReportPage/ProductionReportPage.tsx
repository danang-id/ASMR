import DashboardLayout from "@asmr/layouts/DashboardLayout"
import "@asmr/pages/Dashboard/ProductionReportPage/ProductionReportPage.scoped.css"
import BackButton from "@asmr/components/BackButton"

function ProductionReportPage(): JSX.Element {
	return (
		<DashboardLayout>
			<div className="header">
				<BackButton />&nbsp;&nbsp;
				Production Report
			</div>
			<div className="content">
				<div>
					<div className="warning-box">
						Production Report feature is not available at the moment. Please try again later.
					</div>
				</div>
			</div>
		</DashboardLayout>
	)
}

export default ProductionReportPage
