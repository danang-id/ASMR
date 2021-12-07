//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ProductionsResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class ProductionsResponseModel : DefaultResponseModel<IEnumerable<Roasting>>
{
	public ProductionsResponseModel()
	{
	}

	public ProductionsResponseModel(IEnumerable<Roasting> productions) : base(productions)
	{
	}

	public ProductionsResponseModel(ResponseError error) : base(error)
	{
	}

	public ProductionsResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}