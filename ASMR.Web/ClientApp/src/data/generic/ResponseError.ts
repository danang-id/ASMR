import ErrorCode from "@asmr/data/enumerations/ErrorCode"

interface ResponseError {
	code: ErrorCode
	reason: string
	supportId?: string
}

export default ResponseError
