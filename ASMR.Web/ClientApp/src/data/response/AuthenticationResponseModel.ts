import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import UserWithToken from "@asmr/data/models/UserWithToken"

interface AuthenticationResponseModel extends DefaultResponseModel<UserWithToken> {}

export default AuthenticationResponseModel
