using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ASMR.Common.Constants;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Core.RequestModel;
using ASMR.Core.ResponseModel;
using ASMR.Mobile.Common.Abstractions;
using ASMR.Mobile.Common.Events;
using ASMR.Mobile.Common.Security;
using ASMR.Mobile.Extensions;
using ASMR.Mobile.Services.Abstraction;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using LogLevel = ASMR.Mobile.Common.Logging.LogLevel;

namespace ASMR.Mobile.Common
{
	public class ApplicationState : IApplicationState
	{
        private static IGateService GateService =>
            DependencyService.Get<IGateService>();

        private NormalizedUser _user;


        public ApplicationState()
        {
            InstallId = Guid.Empty;
            LogLevel = LogLevel.None;
        }


        private async Task<bool> IsAppCenterEnabled()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    return false;
                }
                
                if (!await Analytics.IsEnabledAsync())
                {
                    Debug.WriteLine("[WARN] AppCenter Analytics is not available", 
                        GetType().Name);
                    return false;
                }
 
                if (!await Crashes.IsEnabledAsync())
                {
                    Debug.WriteLine("[WARN] AppCenter Crash Reporting is not available.", 
                        GetType().Name);
                    return false;
                }

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception, GetType().Name);
                return false;
            }
        }

        private async Task InitAppCenter()
        {
            if (!await IsAppCenterEnabled())
            {
                return;
            }

            // https://docs.microsoft.com/en-us/appcenter/sdk/other-apis/xamarin#identify-installations
            var installId = await AppCenter.GetInstallIdAsync();
            if (installId is not null)
            {
                InstallId = (Guid)installId;
            }
        }

        private async Task InitAuthentication()
        {
            await UpdateUserDataAsync();
        }

        public Task InitAsync()
        {
            return Task.WhenAll(InitAppCenter(), InitAuthentication());
        }

        public async Task<AuthenticationResponseModel> SignInAsync(string username, string password)
        {
            try
            {
                var result = await GateService.Authenticate(new SignInRequestModel {
                    Username = username,
                    Password = password,
                    RememberMe = true
                });
                
                if (result.Data?.Roles is null || result.Data.Roles.All(role => role.Role != Role.Roaster))
                {
                    throw new Exception("You do not have sufficient role to access this application " +
                        $"(required role: {Role.Administrator}).");
                }
                
                if (result.IsSuccess)
                {
                    User = result.Data;
                    if (!string.IsNullOrEmpty(result.Data.Token))
                    {
                        await TokenManager.SetAsync(ApplicationConstants.JsonWebTokenStorageKey, result.Data.Token);
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                return exception.Cast<AuthenticationResponseModel>();
            }
        }

        public async Task<AuthenticationResponseModel> SignOutAsync()
        {
            try
            {
                var result = await GateService.ClearSession();
                if (!result.IsSuccess)
                {
                    Debug.WriteLine("Sign Out Failed: API Operation Not Success", GetType().Name);
                    return result;
                }
                
                User = null;
                TokenManager.Remove(AuthenticationConstants.CookieName);
                TokenManager.Remove(ApplicationConstants.JsonWebTokenStorageKey);
                
                return result;
            }
            catch (Exception exception)
            {
                return exception.Cast<AuthenticationResponseModel>();
            }
        }

        public async Task<AuthenticationResponseModel> UpdateUserDataAsync()
        {
            try
            {
                var result = await GateService.GetUserPassport();
                if (result.IsSuccess && result.Data is not null)
                {
                    User = result.Data;
                }

                return result;
            }
            catch (Exception exception)
            {
                return exception.Cast<AuthenticationResponseModel>();
            }
        }


        public event EventHandler<AuthenticationEventArgs> AuthenticationChangedEvent = delegate { };

        public Guid InstallId { get; private set; }
        
        public LogLevel LogLevel { get; set; }

        public bool IsAuthenticated => _user is not null;

        public NormalizedUser User {
            get => _user;
            private set {
                var stringValue = value is not null ? JsonSerializer.Serialize(value) : string.Empty;
                Debug.WriteLine($"User Changed: {stringValue}", GetType().Name);
                
                var eventArgs = new AuthenticationEventArgs(value, _user is not null);
                AuthenticationChangedEvent(this, eventArgs);
                
                _user = value;
            }
        }
    }
}