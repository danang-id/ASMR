using System;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Common.Events;
using ASMR.Mobile.Common.Logging;

namespace ASMR.Mobile.Common.Abstractions
{
	public interface IApplicationState
	{
		public Task InitAsync();

		public Task<AuthenticationResponseModel> SignInAsync(string username, string password);

		public Task<AuthenticationResponseModel> SignOutAsync();

		public Task<AuthenticationResponseModel> UpdateUserDataAsync();


		public event EventHandler<AuthenticationEventArgs> AuthenticationChangedEvent;

		public Guid InstallId { get; }

		public bool IsAuthenticated { get; }

		public LogLevel LogLevel { get; set; }

		public NormalizedUser User { get; }
	}
}