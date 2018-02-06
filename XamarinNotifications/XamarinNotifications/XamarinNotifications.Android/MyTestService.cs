using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using System;

namespace XamarinNotifications.Droid
{
    // https://guides.codepath.com/android/starting-background-services

    [Service]
    class MyTestService : IntentService
    {
        public static readonly String ACTION = "com.companyname.XamarinNotifications.MyTestService";

        public MyTestService() : base("MyTestService")
        {
            Log.Info("Isaac MyTestService", "MyTestService");
        }

        protected override void OnHandleIntent(Intent intent)
        {
            Log.Info("Isaac MyTestService", "Service running");
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
                    "The button has been clicked {0} times.", DateTime.Now.ToLongTimeString())); // The message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager =
                (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(new Random().Next(0, 4000), builder.Build());

            // Increment the button press count:
            count++;
        }
    }
}