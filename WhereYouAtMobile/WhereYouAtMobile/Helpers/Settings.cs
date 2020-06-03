using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhereYouAtMobile.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants
        
        private const string AuthTokenKey = "auth_token_key";
        private static readonly string SettingsDefault = string.Empty;
        private const string TrackingIntervalInMillisecondsKey = "tracking_interval_token_key";
        private static readonly int TrackingIntervalDefault = 10000;

        #endregion

        public static string AuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AuthTokenKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AuthTokenKey, value);
            }
        }

        public static string ApiUrl
        {
            get
            {
                return "http://www.whereuat.net";
            }
        }

        public static int TrackingIntervalInMilliseconds
        {
            get
            {
                return AppSettings.GetValueOrDefault(TrackingIntervalInMillisecondsKey, TrackingIntervalDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TrackingIntervalInMillisecondsKey, value);
            }
        }
    }
}
