//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
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