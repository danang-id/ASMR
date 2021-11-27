import Role from "asmr/core/enums/Role"

interface UpdateUserRequestModel {
	firstName: string
	lastName: string
	emailAddress: string
	username: string
	roles: Role[]
}

export default UpdateUserRequestModel
