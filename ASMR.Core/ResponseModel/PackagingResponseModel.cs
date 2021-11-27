//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 10:58 PM
//
// PackagingResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class PackagingResponseModel : DefaultResponseModel<Packaging>
{
	public PackagingResponseModel()
	{
	}

	public PackagingResponseModel(Packaging packaging) : base(packaging)
	{
	}

	public PackagingResponseModel(ResponseError error) : base(error)
	{
	}

	public PackagingResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}