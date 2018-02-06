using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace XamarinNotifications.Droid
{
    [Service]
    class MyTestServiceBom : IntentService
    {
        public static readonly String ACTION = "com.companyname.XamarinNotifications.MyTestService";

        public MyTestServiceBom() : base("test-service")
        {

        }

        protected override void OnHandleIntent(Intent intent)
        {
            // Fetch data passed into the intent on start
            String val = intent.GetStringExtra("foo");
            // Construct an Intent tying it to the ACTION (arbitrary event namespace)
            Intent inObj = new Intent(ACTION);
            // Put extras into the intent as usual
            inObj.PutExtra("resultCode", (int)Result.Ok);
            inObj.PutExtra("resultValue", "My Result Value. Passed in: " + val);
            // Fire the broadcast with intent packaged
            LocalBroadcastManager.GetInstance(this).SendBroadcast(inObj);
            // or sendBroadcast(in) for a normal broadcast;
        }
    }
}