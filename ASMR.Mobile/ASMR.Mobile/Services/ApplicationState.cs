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
using ASMR.Mobile.Common;
using ASMR.Mobile.Extensions;
using ASMR.Mobile.Services.Abstraction;
using ASMR.Mobile.Services.BackEnd;
using ASMR.Mobile.Services.Events;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ASMR.Mobile.Services
{
	public class ApplicationState : IApplicationState
	{
        private static IAuthenticationService AuthenticationService =>
            DependencyService.Get<IAuthenticationService>();

        private NormalizedUser _user;


        public ApplicationState()
        {
            InstallId = Guid.Empty;
            LogLevel = Logging.LogLevel.None;
        }


        private async Task<bool> IsAppCenterEnabled()
        {
            var isAppCenterEnabled = true;
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var isAnalyticsEnabled = await Analytics.IsEnabledAsync();
                    if (!isAnalyticsEnabled)
                    {
                        Debug.WriteLine("[WARN] AppCenter Analytics is not available", GetType().Name);
                        isAppCenterEnabled = false;
                    }
 
                    var isCrashEnabled = await Crashes.IsEnabledAsync();
                    if (!isCrashEnabled)
                    {
                        Debug.WriteLine("[WARN] AppCenter Crash Reporting is not available.", GetType().Name);
                        isAppCenterEnabled = false;
                    }
                }
                else
                {
                    isAppCenterEnabled = false;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"[WARN] Exception while checking if AppCenter is enabled {exception.Message} {exception.StackTrace}",
                    GetType().Name);
                isAppCenterEnabled = false;
            }
            
            return isAppCenterEnabled;
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
            await UpdateUserData();
        }

        public Task Init()
        {
            return Task.WhenAll(new []{
                InitAppCenter(),
                InitAuthentication()
            });
        }

        public async Task<AuthenticationResponseModel> SignIn(string username, string password)
        {
            try
            {
                var result = await AuthenticationService.SignIn(new SignInRequestModel {
                    Username = username,
                    Password = password,
                    RememberMe = true
                });
                if (!result.IsSuccess || result.Data is null)
                {
                    Debug.WriteLine("Sign In Failed: API Operation Not Success", GetType().Name);
                    return result;
                }

                if (result.Data.Roles is null || !result.Data.Roles.Where(role => role.Role == Role.Roaster).Any())
                {
                    Debug.WriteLine("Sign In Failed: Insufficient Role", GetType().Name);
                    throw new Exception("You do not have sufficient role to access this application (required role: Roaster).");
                }
                
                User = result.Data;
                if (!string.IsNullOrEmpty(result.Data.Token))
                {
                    await TokenManager.SetAsync(ApplicationConstants.JsonWebTokenStorageKey, result.Data.Token);
                }

                return result;
            }
            catch (Exception exception)
            {
                return exception.ToResponseModel<AuthenticationResponseModel>();
            }
        }

        public async Task<AuthenticationResponseModel> SignOut()
        {
            try
            {
                var result = await AuthenticationService.SignOut();
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
                return exception.ToResponseModel<AuthenticationResponseModel>();
            }
        }

        public async Task<AuthenticationResponseModel> UpdateUserData()
        {
            try
            {
                var result = await AuthenticationService.GetProfile();
                if (result.IsSuccess && result.Data is not null)
                {
                    User = result.Data;
                }

                return result;
            }
            catch (Exception exception)
            {
                return exception.ToResponseModel<AuthenticationResponseModel>();
            }
        }


        public event EventHandler<AuthenticationEventArgs> AuthenticationChangedEvent = delegate { };

        public Guid InstallId { get; private set; }
        
        public Logging.LogLevel LogLevel { get; set; }

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