using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Auth0.OidcClient;
using Xamarin.Forms;
using WhereYouAtMobile.Messages;
using WhereYouAtMobile.Droid.Services;
using Android.Util;
using static Android.Content.ClipData;
using WhereYouAtMobile.Pages;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace WhereYouAtMobile.Droid
{
    [Activity(Label = "WhereYouAtMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "com.whereyouat",
        DataHost = "feeldaburn.auth0.com",
        DataPathPrefix = "/android/com.whereyouat/callback")]
    [IntentFilter(new[] { Intent.ActionSend },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataMimeType = @"text/plain")]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            CrossCurrentActivity.Current.Init(this, bundle);

            HandleMessageCenterSubscriptions();
            if (!String.IsNullOrEmpty(Intent.GetStringExtra(Intent.ExtraText)))
            {
                string rawTwitterText = Intent.GetStringExtra(Intent.ExtraText);
                string twitterUrl = GetTwitterUrlFromShare(rawTwitterText);
                //Toast.MakeText(this, twitterUrl, ToastLength.Long).Show();
                App.NavPage.PushAsync(new MomentPage(twitterUrl));
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            ActivityMediator.Instance.Send(intent.DataString);

            
            /*if(intent.ClipData != null)
            {
                for(int i=0; i < intent.ClipData.ItemCount; i++)
                {
                    Item item = intent.ClipData.GetItemAt(i);
                    Log.Info("MainActivity", "ClipData item:" + intent.ClipData.GetItemAt(i).Text);
                }
            }*/

            if (!String.IsNullOrEmpty(intent.GetStringExtra(Intent.ExtraText)))
            {
                string rawTwitterText = intent.GetStringExtra(Intent.ExtraText);
                string twitterUrl = GetTwitterUrlFromShare(rawTwitterText);
                //Toast.MakeText(this, twitterUrl, ToastLength.Long).Show();
                App.NavPage.PushAsync(new MomentPage(twitterUrl));
            }
        }

        private void HandleTwitterShareIntent()
        {
            Log.Debug("MainActivity","Intent text: "+ Intent.GetStringExtra(Intent.ExtraText));
            if (!String.IsNullOrEmpty(Intent.GetStringExtra(Intent.ExtraText)))
            {
                Log.Info("MainActivity", "ClipData:" + Intent.ClipData);
                string rawTwitterText = Intent.GetStringExtra(Intent.ExtraText);
                string twitterUrl = GetTwitterUrlFromShare(rawTwitterText);
                Toast.MakeText(this, twitterUrl, ToastLength.Long).Show();
               // App._NavPage.PushAsync(new MomentPage(twitterUrl));
            }
        }

        private String GetTwitterUrlFromShare(String rawTwitterText)
        {
            int start = rawTwitterText.IndexOf("http");
            var url = rawTwitterText.Substring(start);
            var indexOfQuestionMark = url.IndexOf("?");
            if(indexOfQuestionMark > -1)
            {
                url = url.Substring(0, indexOfQuestionMark);
            }
            return url;
        }

        

        private void HandleMessageCenterSubscriptions()
        {

            MessagingCenter.Subscribe<StartTrackingMessage>(this, "StartTrackingMessage", message =>
            {
                //var intent = new Intent(this, typeof(LocationTickService));
                //StartService(intent);

                //StartForegroundServiceComapt<LocationTickService>(this);
                var intent = new Intent(this, typeof(LocationTickService));

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    StartForegroundService(intent);
                }
                else
                {
                    StartService(intent);
                }
            });

            MessagingCenter.Subscribe<StopTrackingMessage>(this, "StopTrackingMessage", message =>
            {
                var intent = new Intent(this, typeof(LocationTickService));
                StopService(intent);
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}

