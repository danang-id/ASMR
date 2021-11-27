//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/22/2021 4:47 PM
//
// UserResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class UserResponseModel : DefaultResponseModel<SanitizedUser>
{
	public UserResponseModel()
	{
	}

	public UserResponseModel(SanitizedUser user) : base(user)
	{
	}

	public UserResponseModel(ResponseError error) : base(error)
	{
	}

	public UserResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}