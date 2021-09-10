using System.Threading.Tasks;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Generic;

namespace ASMR.Mobile.Services
{
	public class ProductService : BackEndService, IProductService
	{
		public ProductService() : base("product")
		{
		}

		public Task<ProductsResponseModel> GetAll()
		{
			var endpoint = GetServiceEndpoint();
			return GetAsync<ProductsResponseModel>(endpoint);
		}

		public Task<ProductResponseModel> GetById(string id)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments(id);
			return GetAsync<ProductResponseModel>(endpoint);
		}
	}
}