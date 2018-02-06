using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace XamarinNotifications.Droid
{
    [BroadcastReceiver]
    class MyAlarmReceiver : BroadcastReceiver
    {
        public static readonly int REQUEST_CODE = 12345;
        public static readonly String ACTION = "com.companyname.XamarinNotifications.MyAlarmReceiver";

        public override void OnReceive(Context context, Intent intent)
        {
            Log.Info("Isaac MyAlarmReceiver", "OnReceive");

            Intent i = new Intent(context, typeof(MyTestService));
            i.PutExtra("foo", "bar");
            context.StartService(i);
        }
    }
}