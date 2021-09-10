using System.Threading.Tasks;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Generic;

namespace ASMR.Mobile.Services
{
	public class BeanService : BackEndService, IBeanService
	{
		public BeanService() : base("bean")
		{
		}

		public Task<BeansResponseModel> GetAll()
		{
			var endpoint = GetServiceEndpoint();
			return GetAsync<BeansResponseModel>(endpoint);
		}

		public Task<BeanResponseModel> GetById(string id)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments(id);
			return GetAsync<BeanResponseModel>(endpoint);
		}
	}
}