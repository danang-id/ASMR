//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 7/10/2021 7:22 PM
//
// ProductionsResponseModel.cs
//
using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel
{
	public class ProductionsResponseModel : DefaultResponseModel<IEnumerable<RoastedBeanProduction>>
	{
		public ProductionsResponseModel()
		{
		}

		public ProductionsResponseModel(IEnumerable<RoastedBeanProduction> productions) : base(productions)
		{
		}

		public ProductionsResponseModel(ResponseError error) : base(error)
		{
		}

		public ProductionsResponseModel(IEnumerable<ResponseError> errors) : base(errors)
		{
		}
	}
}