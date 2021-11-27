//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 7/10/2021 7:22 PM
//
// ProductionResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class ProductionResponseModel : DefaultResponseModel<RoastingSession>
{
	public ProductionResponseModel()
	{
	}

	public ProductionResponseModel(RoastingSession production) : base(production)
	{
	}

	public ProductionResponseModel(ResponseError error) : base(error)
	{
	}

	public ProductionResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}