
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace TotalTech.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        // Launches the startup task
        protected override async void OnResume()
        {
            base.OnResume();
            await SimulateStartup();
            //Task startupWork = new Task(() => { SimulateStartup(); });
            //startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        //async 
        async Task SimulateStartup()
        {
            //await Task.Delay(1000); // Simulate a bit of startup work.
            await Task.Run(() =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                if (Intent.Extras != null)
                    intent.PutExtras(Intent.Extras); // copy push info from splash to main to execute PushNotificationReceived method
                StartActivity(intent);
            });
        }
    }
}
