using System.Threading.Tasks;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.BackEnd;

namespace ASMR.Mobile.Services
{
	public interface IStatusService : IBackEndService
	{
		public Task<StatusResponseModel> GetStatus();
	}

	public class StatusService : BackEndService, IStatusService
	{
		public Task<StatusResponseModel> GetStatus()
		{
			return Get<StatusResponseModel>("status");
		}
	}
}