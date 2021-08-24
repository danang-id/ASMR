using System.Threading.Tasks;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;

namespace ASMR.Mobile.Services.BackEnd
{
	public interface IAuthenticationService : IBackEndService
	{
		public Task<AuthenticationResponseModel> GetProfile();

		public Task<AuthenticationResponseModel> SignIn(SignInRequestModel model);

		public Task<AuthenticationResponseModel> SignOut();
	}
	
	public class AuthenticationService : BackEndService, IAuthenticationService
	{
		public Task<AuthenticationResponseModel> GetProfile()
		{
			return Get<AuthenticationResponseModel>("gate/passport");
		}

		public Task<AuthenticationResponseModel> SignIn(SignInRequestModel model)
		{
			return Post<AuthenticationResponseModel>("gate/authenticate", model);
		}
		
		public Task<AuthenticationResponseModel> SignOut()
		{
			return Post<AuthenticationResponseModel>("gate/exit");
		}
	}
}