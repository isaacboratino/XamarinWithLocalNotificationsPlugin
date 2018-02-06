using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XamarinNotifications.Droid
{
    class MyTestServiceRuim : IntentService
    {
        public MyTestServiceRuim() : base("test-service")
        {

        }

        protected override void OnHandleIntent(Intent intent)
        {
            // Extract the receiver passed into the service
            ResultReceiver rec = (ResultReceiver)intent.GetParcelableExtra("receiver");
            // Extract additional values from the bundle
            String val = intent.GetStringExtra("foo");
            // To send a message to the Activity, create a pass a Bundle
            Bundle bundle = new Bundle();
            bundle.PutString("resultValue", "My Result Value. Passed in: " + val);
            // Here we call send passing a resultCode and the bundle of extras
            rec.Send(Result.Ok, bundle);
        }
    }
}