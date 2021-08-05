enum ErrorCode {
	GenericServerError = 1000,
	GenericClientError = 2000,

	EndpointNotFound = 2100,
	RequestMethodNotAllowed = 2101,
	RequestMediaTypeNotSupported = 2102,

	RequiredParameterNotProvided = 2200,
	InvalidModelFormat = 2201,
	ModelValidationFailed = 2202,

	ResourceNotFound = 2300,
	ResourceEmpty = 2301,

	InvalidAntiforgeryToken = 2400,
	NotAuthenticated = 2401,
	NotAuthorized = 2402,
	AuthenticationFailed = 2403
}

export default ErrorCode
