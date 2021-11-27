import Role from "asmr/core/enums/Role"
import EntityBase from "asmr/core/common/EntityBase"

interface UserRole extends EntityBase {
	role: Role
}

export default UserRole
