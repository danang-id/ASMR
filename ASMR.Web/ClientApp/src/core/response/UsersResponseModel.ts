import ResponseModelBase from "asmr/core/common/ResponseModelBase"
import User from "asmr/core/entities/User"

interface UsersResponseModel extends ResponseModelBase<User[]> {}

export default UsersResponseModel
