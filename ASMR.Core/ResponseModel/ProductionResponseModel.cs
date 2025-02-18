﻿//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ProductionResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class ProductionResponseModel : DefaultResponseModel<Roasting>
{
	public ProductionResponseModel()
	{
	}

	public ProductionResponseModel(Roasting production) : base(production)
	{
	}

	public ProductionResponseModel(ResponseError error) : base(error)
	{
	}

	public ProductionResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}