using System.Threading.Tasks;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;

namespace ASMR.Mobile.Services.BackEnd
{
	public interface IAuthenticationService : IBackEndService
	{
		public Task<AuthenticationResponseModel> GetProfile();

		public Task<AuthenticationResponseModel> ResendEmailAddressConfirmation(
			ResendEmailAddressConfirmationRequestModel model);
		
		public Task<AuthenticationResponseModel> SignIn(SignInRequestModel model);

		public Task<AuthenticationResponseModel> SignOut();
	}
	
	public class AuthenticationService : BackEndService, IAuthenticationService
	{
		public Task<AuthenticationResponseModel> GetProfile()
		{
			return GetAsync<AuthenticationResponseModel>("gate/passport");
		}

		public Task<AuthenticationResponseModel> ResendEmailAddressConfirmation(
			ResendEmailAddressConfirmationRequestModel model)
		{
			return PostAsync<AuthenticationResponseModel>("gate/email-address/resend-confirmation", model);
		}

		public Task<AuthenticationResponseModel> SignIn(SignInRequestModel model)
		{
			return PostAsync<AuthenticationResponseModel>("gate/authenticate", model);
		}
		
		public Task<AuthenticationResponseModel> SignOut()
		{
			return PostAsync<AuthenticationResponseModel>("gate/exit");
		}
	}
}