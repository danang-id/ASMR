
import BackButton from "@asmr/components/BackButton"
import DashboardLayout from "@asmr/layouts/DashboardLayout"
import "@asmr/pages/Dashboard/ProductTransactionsPage/ProductTransactionsPage.scoped.css"

function ProductTransactionsPage(): JSX.Element {
	return (
		<DashboardLayout>
			<div className="header">
				<BackButton />&nbsp;&nbsp;
				Product Transactions
			</div>
			<div className="content">
				<div>
					<div className="warning-box">
						Product Transactions feature is not available at the moment. Please try again later.
					</div>
				</div>
			</div>
		</DashboardLayout>
	)
}

export default ProductTransactionsPage
