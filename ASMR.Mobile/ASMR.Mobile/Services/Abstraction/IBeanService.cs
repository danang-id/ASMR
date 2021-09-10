using System.Threading.Tasks;
using ASMR.Core.ResponseModel;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface IBeanService : IHttpService
	{
		public Task<BeansResponseModel> GetAll();

		public Task<BeanResponseModel> GetById(string id);
	}
}