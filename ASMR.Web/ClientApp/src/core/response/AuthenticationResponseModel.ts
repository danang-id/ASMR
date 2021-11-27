import ResponseModelBase from "asmr/core/common/ResponseModelBase"
import UserWithToken from "asmr/core/entities/UserWithToken"

interface AuthenticationResponseModel extends ResponseModelBase<UserWithToken> {}

export default AuthenticationResponseModel
