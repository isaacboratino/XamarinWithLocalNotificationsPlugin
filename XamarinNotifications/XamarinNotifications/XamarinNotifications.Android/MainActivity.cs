using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;

namespace XamarinNotifications.Droid
{
    [Activity(Label = "XamarinNotifications", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            //StartService(new Intent(this, typeof(SimpleStartedService)));

            scheduleAlarm();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public void scheduleAlarm()
        {
            Log.Info("Isaac MainActivity", "scheduleAlarm");

            // Construct an intent that will execute the AlarmReceiver
            Intent intent = new Intent(Application.Context, typeof(MyAlarmReceiver));

            // Create a PendingIntent to be triggered when the alarm goes off
            PendingIntent pIntent = PendingIntent.GetBroadcast(this, MyAlarmReceiver.REQUEST_CODE, intent, PendingIntentFlags.UpdateCurrent);

            // Setup periodic alarm every every half hour from this point onwards
            long firstMillis = DateTime.Now.Millisecond; // alarm is set right away
            AlarmManager alarm = (AlarmManager)this.GetSystemService(Context.AlarmService);

            // First parameter is the type: ELAPSED_REALTIME, ELAPSED_REALTIME_WAKEUP, RTC_WAKEUP
            // Interval can be INTERVAL_FIFTEEN_MINUTES, INTERVAL_HALF_HOUR, INTERVAL_HOUR, INTERVAL_DAY
            //alarm.SetInexactRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime(), 2000, pIntent);

            if ((int)Build.VERSION.SdkInt >= 23) {
                alarm.SetRepeating(AlarmType.RtcWakeup, firstMillis, 100, pIntent);
            } else if ((int)Build.VERSION.SdkInt >= 19) {
                alarm.SetRepeating(AlarmType.RtcWakeup, firstMillis, 100, pIntent);
            } else {
                alarm.SetRepeating(AlarmType.RtcWakeup, firstMillis, 100, pIntent);
            }

            Log.Info("Isaac scheduleAlarm", "SetInexactRepeating");
        }
    }
}

