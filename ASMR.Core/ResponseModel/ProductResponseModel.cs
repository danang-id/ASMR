//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 10:58 PM
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