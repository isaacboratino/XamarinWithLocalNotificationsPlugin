using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;

namespace XamarinNotifications.Droid
{
    [Service]
    public class SimpleStartedService : Service
    {
        static readonly string TAG = "X:" + typeof(SimpleStartedService).Name;
        static readonly int TimerWait = 8000;
        Timer timer;
        DateTime startTime;
        bool isStarted = false;
        Intent intent;
        private LocalBinder mBinder;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public class LocalBinder : Binder
        {
            SimpleStartedService instanceService;

            public LocalBinder(SimpleStartedService instanceService)
            {
                this.instanceService = instanceService;
            }

            public SimpleStartedService getService()
            {
                return instanceService;
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            mBinder = new LocalBinder(this);
            this.intent = intent;

            Log.Debug(TAG, $">>1<<OnStartCommand called at {startTime}, flags={flags}, startid={startId}");
            if (isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                Log.Debug(TAG, $">>2<<This service was already started, it's been running for {runtime:c}.");
            }
            else
            {
                startTime = DateTime.UtcNow;
                Log.Debug(TAG, $">>3<<Starting the service, at {startTime}.");
                timer = new Timer(HandleTimerCallback, startTime, 0, TimerWait);
                isStarted = true;
            }
            //ButtonOnClick();
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // This is a started service, not a bound service, so we just return null.
            return null;
        }

        public override void OnDestroy()
        {           
            /*
            timer.Dispose();
            timer = null;
            isStarted = false;

            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"Simple Service destroyed at {DateTime.UtcNow} after running for {runtime:c}.");
            base.OnDestroy();
            */
        }

        void HandleTimerCallback(object state)
        {
            TimeSpan runTime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $">>4<<This service has been running for {runTime:c} (since ${state}).");
            ButtonOnClick();
        }

        int count = 0;
        private void ButtonOnClick()
        {
            // Pass the current button press count value to the next activity:
            Bundle valuesForActivity = new Bundle();
            valuesForActivity.PutInt("count", count);

            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(this, typeof(MainActivity));

            // Pass some values to SecondActivity:
            resultIntent.PutExtras(valuesForActivity);

            // Construct a back stack for cross-task navigation:
            Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            //PendingIntent resultPendingIntent =
            //    stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentTitle("Button Clicked")      // Set its title
                .SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.icon)  // Display this icon
                .SetContentText(String.Format(
                    "The button has been clicked {0} times.", count)); // The message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager =
                (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(2, builder.Build());

            // Increment the button press count:
            count++;
        }
    }
}