interface ResetPasswordRequestModel {
	id: string
	emailAddress: string
	token: string
	password: string
	passwordConfirmation: string
}

export default ResetPasswordRequestModel
