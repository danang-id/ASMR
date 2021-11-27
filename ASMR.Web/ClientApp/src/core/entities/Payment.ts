import EntityBase from "asmr/core/common/EntityBase"
import PaymentMethod from "asmr/core/enums/PaymentMethod"
import PaymentStatus from "asmr/core/enums/PaymentStatus"

interface Payment extends EntityBase {
	method: PaymentMethod
	status: PaymentStatus
	amount: number
}

export default Payment
