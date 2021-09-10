using System.Threading.Tasks;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Generic;

namespace ASMR.Mobile.Services
{
	public class ProductionService : BackEndService, IProductionService
	{
		public ProductionService() : base("production")
		{
		}
		
		public Task<ProductionsResponseModel> GetAll(bool showMine = false)
		{
			var endpoint = GetServiceEndpoint().SetQueryParam(nameof(showMine), showMine);
			return GetAsync<ProductionsResponseModel>(endpoint);
		}
		
		public Task<ProductionResponseModel> GetById(string id)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments(id);
			return GetAsync<ProductionResponseModel>(endpoint);
		}
		
		public Task<ProductionResponseModel> Start(StartProductionRequestModel model)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("start");
			return PostAsync<ProductionResponseModel, StartProductionRequestModel>(
				endpoint, model);
		}
		
		public Task<ProductionResponseModel> Finalize(string id, FinalizeProductionRequestModel model)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("finalize", id);
			return PostAsync<ProductionResponseModel, FinalizeProductionRequestModel>(
				endpoint, model);
		}
		
		public Task<ProductionResponseModel> Cancel(string id)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("cancel", id);
			return DeleteAsync<ProductionResponseModel>(endpoint);
		}
	}
}