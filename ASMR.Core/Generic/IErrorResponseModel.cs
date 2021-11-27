//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 1:24 PM
//
// IErrorResponseModel.cs
//

using System.Collections.Generic;

namespace ASMR.Core.Generic;

public interface IErrorResponseModel
{
	public IEnumerable<ResponseError> Errors { get; set; }
}