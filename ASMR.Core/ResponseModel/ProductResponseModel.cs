//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// ProductResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class ProductResponseModel : DefaultResponseModel<Product>
{
	public ProductResponseModel()
	{
	}

	public ProductResponseModel(Product product) : base(product)
	{
	}

	public ProductResponseModel(ResponseError error) : base(error)
	{
	}

	public ProductResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}