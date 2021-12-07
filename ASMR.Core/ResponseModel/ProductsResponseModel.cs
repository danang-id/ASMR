//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ProductsResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class ProductsResponseModel : DefaultResponseModel<IEnumerable<Product>>
{
	public ProductsResponseModel()
	{
	}

	public ProductsResponseModel(IEnumerable<Product> products) : base(products)
	{
	}

	public ProductsResponseModel(ResponseError error) : base(error)
	{
	}

	public ProductsResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}