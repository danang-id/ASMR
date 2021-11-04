
	firstName: string
	lastName: string
	emailAddress: string
	username: string
	image: string
	isEmailConfirmed: boolean
	isApproved: boolean
	isWaitingForApproval: boolean
	roles: UserRole[]import Role from "@asmr/data/enumerations/Role"
import DefaultModel from "@asmr/data/generic/DefaultModel"

interface UserRole extends DefaultModel {
	role: Role
}

export default UserRole
