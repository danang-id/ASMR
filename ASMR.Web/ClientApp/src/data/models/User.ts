import DefaultModel from "@asmr/data/generic/DefaultModel"
import UserRole from "@asmr/data/models/UserRole"

export const EmptyUser: User = {
	id: "",
	firstName: "",
	lastName: "",
	emailAddress: "",
	username: "",
	image: "",
	roles: [],
	createdAt: new Date(),
	lastUpdatedAt: new Date()
}

interface User extends DefaultModel {
	firstName: string
	lastName: string
	emailAddress: string
	username: string
	image: string
	roles: UserRole[]
}

export default User
