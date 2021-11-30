import ErrorInformation from "asmr/core/common/ErrorInformation"

interface ResponseModelBase<TData = any> {
	isSuccess: boolean
	message?: string
	errors?: ErrorInformation[]
	data?: TData
}

export default ResponseModelBase
