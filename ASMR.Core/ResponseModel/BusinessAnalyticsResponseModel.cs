//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 4:15 PM
//
// BusinessAnalyticsResponseModel.cs
//

using System.Collections.Generic;
using ASMR.Core.Entities;
using ASMR.Core.Generic;

namespace ASMR.Core.ResponseModel;

public class BusinessAnalyticsResponseModel : DefaultResponseModel<IEnumerable<BusinessAnalytic>>
{
	public BusinessAnalyticsResponseModel()
	{
	}

	public BusinessAnalyticsResponseModel(IEnumerable<BusinessAnalytic> businessAnalytics) : base(businessAnalytics)
	{
	}

	public BusinessAnalyticsResponseModel(ResponseError error) : base(error)
	{
	}

	public BusinessAnalyticsResponseModel(IEnumerable<ResponseError> errors) : base(errors)
	{
	}
}