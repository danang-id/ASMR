import DefaultModel from "@asmr/data/generic/DefaultModel"
import PaymentMethod from "@asmr/data/enumerations/PaymentMethod"
import PaymentStatus from "@asmr/data/enumerations/PaymentStatus"

interface Payment extends DefaultModel {
	method: PaymentMethod
	status: PaymentStatus
	amount: number
}

export default Payment
