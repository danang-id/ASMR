//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// IResponseModel.cs
//

namespace ASMR.Core.Generic;

public interface IResponseModel : IResponseModel<object>
{
}

public interface IResponseModel<T> where T : class
{
	public bool IsSuccess { get; set; }

	public string Message { get; set; }

	public T Data { get; set; }
}