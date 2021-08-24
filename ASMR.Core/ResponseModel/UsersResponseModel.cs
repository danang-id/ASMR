//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/22/2021 4:47 PM
//
// UsersResponseModel.cs
//
using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel
{
    public class UsersResponseModel : DefaultResponseModel<IEnumerable<NormalizedUser>>
    {
        public UsersResponseModel() 
        {
        }

        public UsersResponseModel(IEnumerable<NormalizedUser> users) : base(users)
        {
        }

        public UsersResponseModel(ResponseError error) : base(error)
        {
        }

        public UsersResponseModel(IEnumerable<ResponseError> errors) : base(errors)
        {
        }
    }
}
