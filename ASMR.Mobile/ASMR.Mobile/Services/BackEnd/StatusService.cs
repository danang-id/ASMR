using System.Threading.Tasks;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;

namespace ASMR.Mobile.Services.BackEnd
{
	public interface IStatusService : IBackEndService
	{
		public Task<StatusResponseModel> GetStatus();
	}

	public class StatusService : BackEndService, IStatusService
	{
		public Task<StatusResponseModel> GetStatus()
		{
			return GetAsync<StatusResponseModel>("status");
		}
	}
}