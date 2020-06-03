using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhereYouAtMobile.Models;

namespace WhereYouAtMobile.Services
{
    class LocationService
    {
        private PositionService positionService;

        public LocationService()
        {
            this.positionService = new PositionService();
        }

        public async Task<Location> RetrieveLocationAsync(String message, String twitterId)
        {
            var position = await this.positionService.RetrievePositionAsync();

            var location = new Location(position.Longitude, position.Latitude, epochTimeStamp(), message, twitterId);

            return location;
        }

        private long epochTimeStamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}
