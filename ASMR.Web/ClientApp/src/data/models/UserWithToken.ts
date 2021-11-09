import User from "@asmr/data/models/User"

interface UserWithToken extends User {
	token?: string
}

export default UserWithToken
