import Role from "@asmr/data/enumerations/Role"
import DefaultModel from "@asmr/data/generic/DefaultModel"

interface UserRole extends DefaultModel {
	role: Role
}

export default UserRole
