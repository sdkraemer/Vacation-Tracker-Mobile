using System;
using System.Collections.Generic;
using System.Text;

namespace WhereYouAtMobile.Models
{
    public class Location
    {
        public double Longitude { get; private set; }
        public double Latitude { get; private set; }
        public long Time { get; private set; }
        public String Message { get; private set; }
        public String TweetId { get; private set; }

        public Location(double longitude, double latitude, long time, String message, String tweetId)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Time = time;
            this.Message = message;
            this.TweetId = tweetId;
        }

    }
}
