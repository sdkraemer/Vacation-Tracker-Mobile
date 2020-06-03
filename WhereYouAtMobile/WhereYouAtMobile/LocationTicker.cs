using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhereYouAtMobile.Helpers;
using WhereYouAtMobile.Messages;
using WhereYouAtMobile.Services;
using Xamarin.Forms;

namespace WhereYouAtMobile
{
    public class LocationTicker
    {
        private LocationService locationService;
        private int locationRequestedCounter;
        private LocationRestService locationRestService;

        public LocationTicker()
        {
            this.locationService = new LocationService();
            this.locationRequestedCounter = 0;
            this.locationRestService = new LocationRestService(new ResilientRequestProvider());
        }

        public async Task Run(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    await sendLocationTickMessageAsync();
                    await Task.Delay(Settings.TrackingIntervalInMilliseconds);
                    
                    this.locationRequestedCounter++;
                }
            }, token);
        }

        private async Task sendLocationTickMessageAsync()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Debug.WriteLine("****GPS fetching location");
            var location = await this.locationService.RetrieveLocationAsync("", "");
            stopWatch.Stop();
            var message = new LocationReceivedMessage()
            {
                ReceivedAt = DateTime.Now
            };
            MessagingCenter.Send<LocationReceivedMessage>(message, "LocationReceivedMessage");
            Debug.WriteLine("****After requesting location. Time elapsed: "+stopWatch.ElapsedMilliseconds+"ms\n");

            /*var message = new LocationTickMessage
            {
                Message = this.locationRequestedCounter.ToString(),
                Location = location,
                RetrievedAt = DateTime.Now
            };*/
            await this.locationRestService.SaveLocationAsync(location);

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //MessagingCenter.Send<LocationTickMessage>(message, "LocationTickMessage");
            //});
        }
    }
}
