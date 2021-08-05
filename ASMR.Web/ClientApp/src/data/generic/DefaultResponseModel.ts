import ResponseError from "./ResponseError"

interface DefaultResponseModel<TData = any> {
	isSuccess: boolean
	message?: string
	errors?: ResponseError[]
	data?: TData
}

export default DefaultResponseModel
