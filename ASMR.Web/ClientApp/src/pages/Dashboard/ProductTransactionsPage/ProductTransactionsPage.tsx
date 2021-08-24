
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
						We are sorry, this feature is not currently available at the moment.<br/>
						Please try again later.
					</div>
				</div>
			</div>
		</DashboardLayout>
	)
}

export default ProductTransactionsPage
