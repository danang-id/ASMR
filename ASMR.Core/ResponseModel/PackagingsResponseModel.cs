//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// 
// PackagingsResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class PackagingsResponseModel : DefaultResponseModel<IEnumerable<Packaging>>
{
	public PackagingsResponseModel()
	{
	}

	public PackagingsResponseModel(IEnumerable<Packaging> packagings) : base(packagings)
	{
	}

	public PackagingsResponseModel(ResponseError error) : base(error)
	{
	}

	public PackagingsResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}