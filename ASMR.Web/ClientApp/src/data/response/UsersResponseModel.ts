import DefaultResponseModel from "@asmr/data/generic/DefaultResponseModel"
import User from "@asmr/data/models/User"

interface UsersResponseModel extends DefaultResponseModel<User[]> {}

export default UsersResponseModel
