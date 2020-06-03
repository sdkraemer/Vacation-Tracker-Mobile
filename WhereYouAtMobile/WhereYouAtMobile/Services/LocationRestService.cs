using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WhereYouAtMobile.Helpers;
using WhereYouAtMobile.Messages;
using WhereYouAtMobile.Models;
using Xamarin.Forms;

namespace WhereYouAtMobile.Services
{
    class LocationRestService : ILocationRestService
    {
        private IRequestProvider requestProvider;

        public LocationRestService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task SaveLocationAsync(Location location)
        {
            try
            {
                bool result;
                Debug.WriteLine("POSTing location to API");
                result = await this.requestProvider.PostAsync(Settings.ApiUrl + "/api/locations", createJsonFromLocation(location));
                if(result)
                {
                    Debug.WriteLine("Location saved successfully.");
                    var message = new LocationSentToApiSuccessMessage()
                    {
                        ReceivedAt = DateTime.Now
                    };
                    MessagingCenter.Send<LocationSentToApiSuccessMessage>(message, "LocationSentToApiSuccessMessage");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("ERROR: {0}", ex.Message);
            }
        }

        private JObject createJsonFromLocation(Location location)
        {
            var json = new JObject();
            json.Add("long", location.Longitude);
            json.Add("lat", location.Latitude);
            json.Add("time", location.Time);
            json.Add("message", location.Message);
            json.Add("twitterUrl", location.TweetId);
            return json;
        }
    }
}
