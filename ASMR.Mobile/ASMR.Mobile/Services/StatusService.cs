using System.Threading.Tasks;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Generic;

namespace ASMR.Mobile.Services
{
	public class StatusService : BackEndService, IStatusService
	{
		public StatusService() : base("status")
		{
		}
		
		public Task<StatusResponseModel> GetStatus()
		{
			var endpoint = GetServiceEndpoint();
			return GetAsync<StatusResponseModel>(endpoint);
		}
	}
}