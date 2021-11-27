//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/24/2021 12:07 AM
//
// StatusResponseModel.cs
//

using System;
using System.Collections.Generic;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class StatusResponseModel : DefaultResponseModel
{
	public StatusResponseModel()
	{
	}

	public StatusResponseModel(bool ok, Version application, Version runtime)
		: base(new
		{
			Status = ok ? "OK" : "Not OK",
			Version = new
			{
				Application = application,
				Runtime = runtime
			}
		})
	{
	}

	public StatusResponseModel(ResponseError error) : base(error)
	{
	}

	public StatusResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}