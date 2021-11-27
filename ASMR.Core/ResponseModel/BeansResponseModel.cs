//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 4:15 PM
//
// BeansResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class BeansResponseModel : DefaultResponseModel<IEnumerable<Bean>>
{
	public BeansResponseModel()
	{
	}

	public BeansResponseModel(IEnumerable<Bean> beans) : base(beans)
	{
	}

	public BeansResponseModel(ResponseError error) : base(error)
	{
	}

	public BeansResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}