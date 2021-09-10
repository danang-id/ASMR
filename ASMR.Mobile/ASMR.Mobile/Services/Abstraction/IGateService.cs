using System.Threading.Tasks;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;

namespace ASMR.Mobile.Services.Abstraction
{
	public interface IGateService : IHttpService
	{
		public Task<AuthenticationResponseModel> ResendEmailAddressConfirmation(
			ResendEmailAddressConfirmationRequestModel model);
		
		public Task<AuthenticationResponseModel> Authenticate(SignInRequestModel model);

		public Task<AuthenticationResponseModel> ClearSession();
		
		public Task<AuthenticationResponseModel> ForgetPassword(ForgetPasswordRequestModel model);
		
		public Task<AuthenticationResponseModel> GetUserPassport();
	}
}