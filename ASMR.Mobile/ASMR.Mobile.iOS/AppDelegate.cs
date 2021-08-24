using System.Net.Http;
using ASMR.Common.Net.Http;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace ASMR.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register(nameof(AppDelegate))]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            NativeHttpClient.DefaultClientHandler = new NSUrlSessionHandler()
            {
                CookieContainer = NativeHttpClient.CookieContainer,
                UseCookies = true
            };
            
            Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
