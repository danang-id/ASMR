using System.Threading.Tasks;
using ASMR.Core.ResponseModel;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface IProductionService : IHttpService
	{
		public Task<ProductionsResponseModel> GetAll(bool showMine = false);

		public Task<ProductionResponseModel> GetById(string id);
	}
}