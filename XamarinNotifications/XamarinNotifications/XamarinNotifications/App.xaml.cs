using Plugin.Notifications;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinNotifications
{
    public partial class App : Application
	{
        public static bool IsInBackgrounded { get; private set; }

        public App()
        {
            Notification.DefaultTitle = "Test Title";
            var btnPermission = new Button { Text = "Request Permission" };
            btnPermission.Command = new Command(async () =>
            {
                var result = await CrossNotifications.Current.RequestPermission();
                btnPermission.Text = result ? "Permission Granted" : "Permission Denied";
            });

            this.MainPage = new NavigationPage(new ContentPage
            {
                Title = "Notifications",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        btnPermission,
                        new Button
                        {
                            Text = "Set Badge",
                            Command = new Command(() => CrossNotifications.Current.SetBadge(new Random().Next(100)))
                        },
                        new Button
                        {
                            Text = "Clear Badge",
                            Command = new Command(() => CrossNotifications.Current.SetBadge(0))
                        },
                        new Button
                        {
                            Text = "Press This & Exit App within 10 seconds",
                            Command = new Command(() =>
                                CrossNotifications.Current.Send(new Notification
                                {
                                    Id = 900,
                                    Title = "HELLO!",
                                    Message = "Hello from the ACR Sample Notification App",
                                    Vibrate = true,
                                    When = TimeSpan.FromSeconds(10)
                                })
                            )
                        },
                        new Button
                        {
                            
                            Text = "Multiple Timed Messages (10 messages x 5 seconds apart)",
                            Command = new Command(() =>
                            {
                                CrossNotifications.Current.Send(new Notification
                                {
                                    Id = 0,
                                    Title = "Samples",
                                    Message = "Starting Sample Schedule Notifications"
                                });
                                for (var i = 1; i < 11; i++)
                                {
                                   Task.Factory.StartNew (() => {
                                        var seconds = i * 5;
                                        var id = CrossNotifications.Current.Send(new Notification
                                        {
                                            Id = i,
                                            Message = $"Message {i}",
                                            When = TimeSpan.FromSeconds(seconds)
                                        });
                                        Debug.WriteLine($"Notification ID: {id}");
                                    });
                                    Task.Delay(300);
                                }
                            })
                        },
                        new Button
                        {
                            Text = "Cancel All Notifications",
                            Command = new Command(() => CrossNotifications.Current.CancelAll())
                        },
                        new Button
                        {
                            Text = "Vibrate",
                            Command = new Command(() => CrossNotifications.Current.Vibrate())
                        }
                    }
                }
            });
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

        protected override void OnResume()
        {
            base.OnResume();
            App.IsInBackgrounded = false;
        }


        protected override void OnSleep()
        {
            base.OnSleep();
            for (int i = 0; i < 10; i++)
            {
                Task.Delay(2000).Wait();
                CrossNotifications.Current.Send(new Notification
                {
                    Id = i,
                    Title = "HELLO! "+i,
                    Message = "Iterator "+i,
                    Vibrate = true,
                });
            }            
            App.IsInBackgrounded = true;
        }

        public static async Task Sleep(int ms)
        {
            await Task.Delay(ms);
        }
    }
}
