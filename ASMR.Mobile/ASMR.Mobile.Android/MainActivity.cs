using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using ASMR.Common.Net.Http;
using Xamarin.Android.Net;
using Xamarin.Forms.Platform.Android;

namespace ASMR.Mobile.Droid
{
    [Activity]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            NativeHttpClient.DefaultClientHandler = new AndroidClientHandler
            {
                CookieContainer = NativeHttpClient.CookieContainer,
                UseCookies = true
            };
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode,
            string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform
                .OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}