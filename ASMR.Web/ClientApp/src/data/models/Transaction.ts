import DefaultModel from "@asmr/data/generic/DefaultModel"
import TransactionItem from "@asmr/data/models/TransactionItem"
import User from "@asmr/data/models/User"
import TransactionStatus from "@asmr/data/enumerations/TransactionStatus"
import Payment from "@asmr/data/models/Payment"

interface Transaction extends DefaultModel {
	items: TransactionItem[]
	payment: Payment
	status: TransactionStatus
	server: User
}

export default Transaction
