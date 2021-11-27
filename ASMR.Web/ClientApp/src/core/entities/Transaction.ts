import EntityBase from "asmr/core/common/EntityBase"
import TransactionItem from "asmr/core/entities/TransactionItem"
import TransactionStatus from "asmr/core/enums/TransactionStatus"
import Payment from "asmr/core/entities/Payment"

interface Transaction extends EntityBase {
	items: TransactionItem[]
	payment: Payment
	status: TransactionStatus
	userId: string
}

export default Transaction
