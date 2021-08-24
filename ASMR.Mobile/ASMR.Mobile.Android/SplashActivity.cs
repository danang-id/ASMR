using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace ASMR.Mobile.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : FormsAppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new(SimulateStartup);
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        private async void SimulateStartup()
        {
            await Task.Delay(2000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
