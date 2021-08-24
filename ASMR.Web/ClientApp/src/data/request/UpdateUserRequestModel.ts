import Role from "@asmr/data/enumerations/Role"

interface UpdateUserRequestModel {
	firstName: string
	lastName: string
	emailAddress: string
	username: string
	roles: Role[]
}

export default UpdateUserRequestModel
