//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 1:37 PM
//
// AuthenticationResponseModel.cs
//
using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel
{
    public class AuthenticationResponseModel : DefaultResponseModel<NormalizedUserWithToken>
    {
        public AuthenticationResponseModel()
        {
        }

        public AuthenticationResponseModel(NormalizedUserWithToken user) : base(user)
        {
        }

        public AuthenticationResponseModel(ResponseError error) : base(error)
        {
        }

        public AuthenticationResponseModel(IEnumerable<ResponseError> errors) : base(errors)
        {
        }
    }
}
