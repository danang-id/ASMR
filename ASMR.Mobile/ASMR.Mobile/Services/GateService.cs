using System.Threading.Tasks;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.Generic;

namespace ASMR.Mobile.Services
{
	public class GateService : BackEndService, IGateService
	{
		public GateService() : base("gate")
		{
		}
		
		public Task<AuthenticationResponseModel> ResendEmailAddressConfirmation(
			ResendEmailAddressConfirmationRequestModel model)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("email-address", "resend-confirmation");
			return PostAsync<AuthenticationResponseModel, ResendEmailAddressConfirmationRequestModel>(
				endpoint, model);
		}
		
		public Task<AuthenticationResponseModel> Authenticate(SignInRequestModel model)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("authenticate");
			return PostAsync<AuthenticationResponseModel, SignInRequestModel>(
				endpoint, model);
		}
		
		public Task<AuthenticationResponseModel> ClearSession()
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("exit");
			return PostAsync<AuthenticationResponseModel>(endpoint);
		}
		
		public Task<AuthenticationResponseModel> ForgetPassword(ForgetPasswordRequestModel model)
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("password", "forget");
			return PostAsync<AuthenticationResponseModel, ForgetPasswordRequestModel>(
				endpoint, model);
		}
		
		public Task<AuthenticationResponseModel> GetUserPassport()
		{
			var endpoint = GetServiceEndpoint().AppendPathSegments("passport");
			return GetAsync<AuthenticationResponseModel>(endpoint);
		}

		
	}
}