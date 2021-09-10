using System.Threading.Tasks;
using ASMR.Core.ResponseModel;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface IStatusService : IHttpService
	{
		public Task<StatusResponseModel> GetStatus();
	}
}