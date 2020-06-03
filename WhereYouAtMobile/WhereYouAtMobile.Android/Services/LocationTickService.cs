using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using WhereYouAtMobile.Messages;
using Xamarin.Forms;

namespace WhereYouAtMobile.Droid.Services
{
    [Service]
    class LocationTickService : Service
    {
        CancellationTokenSource _cts;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string PRIMARY_CHANNEL = "default";

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();
            Task.Run(() =>
            {
                try
                {
                    //var message = new LocationTickMessage();
                    //MessagingCenter.Send<LocationTickMessage>(message, "LocationTickMessage");
                    var locationTicker = new LocationTicker();
                    locationTicker.Run(_cts.Token).Wait();
                }
                catch (Android.OS.OperationCanceledException)
                {

                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        Log.Info("LocationTickService", "LocationTickService was cancelled");
                        var message = new LocationTrackingCancelledMessage();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send(message, "LocationTrackingCancelledMessage");
                        });
                    }
                }
            }, _cts.Token);
            /*var notification = new Notification.Builder(this)
                .SetContentTitle(Resources.GetString(Resource.String.ApplicationName))
                .SetContentText("Notification Text Here")
                .SetSmallIcon(Resource.Drawable.notification_icon_background)
                .SetContentIntent(BuildIntentToShowMainActivity())
                //.SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                //.AddAction(BuildRestartTimerAction())
                //.AddAction(BuildStopServiceAction())
                .Build();*/
            if(Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                createNotificationChannel();
            }


            var notification = new Notification.Builder(this)
                .SetChannelId(PRIMARY_CHANNEL)
                .SetContentTitle("Whereuat")
                .SetContentText("Tracking running!")
                .SetSmallIcon(Resource.Drawable.ic_location_city_blue_800_18dp)
                .Build();

            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);

            return StartCommandResult.Sticky;
        }

        private void createNotificationChannel()
        {
            var channel = new NotificationChannel(PRIMARY_CHANNEL, "Primary Channel", NotificationImportance.Default);
            NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
            manager.CreateNotificationChannel(channel);
        }

    PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            //notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            //notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }
        }
    }
}