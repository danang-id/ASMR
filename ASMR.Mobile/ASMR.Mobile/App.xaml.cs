using ASMR.Common.Net.Http;
using ASMR.Mobile.Common;
using ASMR.Mobile.Common.Abstractions;
using ASMR.Mobile.Common.Logging;
using ASMR.Mobile.Common.Net;
using ASMR.Mobile.Extensions;
using ASMR.Mobile.Services;
using ASMR.Mobile.Services.Abstraction;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using LogLevel = ASMR.Mobile.Common.Logging.LogLevel;

namespace ASMR.Mobile
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            
            DependencyService.Register<IApplicationState, ApplicationState>();
            DependencyService.Register<ILogging, Logging>();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IBeanService, BeanService>();
            DependencyService.Register<IGateService, GateService>();
            DependencyService.Register<IProductionService, ProductionService>();
            DependencyService.Register<IProductService, ProductService>();
            DependencyService.Register<IStatusService, StatusService>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
	        await NativeHttpClient.CookieContainer.AddApplicationTokens(BackEndUrl.BaseUri);
            AppCenter.Start($"android={AppSettingsManager.Settings.AppCenter.SecretKeys.Android};" +
                            $"ios={AppSettingsManager.Settings.AppCenter.SecretKeys.iOS};",
                typeof(Analytics), typeof(Crashes));
 
            var applicationState = DependencyService.Get<IApplicationState>();

#if DEBUG
            applicationState.LogLevel = LogLevel.Verbose;
#else
            applicationState.LogLevel = LogLevel.Information;
#endif

            await applicationState.InitAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
