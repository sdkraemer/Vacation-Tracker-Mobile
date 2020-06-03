using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhereYouAtMobile.Services
{
    class PositionService
    {
        private IGeolocator geolocator;

        public PositionService()
        {
            this.geolocator = CrossGeolocator.Current;
            this.geolocator.DesiredAccuracy = 500;
            
        }

        public async Task<Position> RetrievePositionAsync()
        {
            //var hasPermission = await Utils.CheckPermissions(Permission.Location);
            //if (!hasPermission)
            //    return null;
            var result = await Utils.CheckPermissions(Permission.Location);
            if(result)
            {
                var position = await this.geolocator.GetPositionAsync(TimeSpan.FromSeconds(10));//TODO: change timeout later, save in settings;
                return position;
            }
            else
            {
                return null;
            }
        }
    }
}
