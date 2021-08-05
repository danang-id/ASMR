import Role from "@asmr/data/enumerations/Role"

interface CreateUserRequestModel {
	firstName: string
	lastName: string
	username: string
	password: string
	passwordConfirmation: string
	roles: Role[]
}

export default CreateUserRequestModel
