using System.Threading.Tasks;
using ASMR.Core.ResponseModel;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface IProductService : IHttpService
	{
		public Task<ProductsResponseModel> GetAll();

		public Task<ProductResponseModel> GetById(string id);
	}
}