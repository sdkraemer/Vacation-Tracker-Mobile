using System;
using System.Collections.Generic;
using System.Text;
using WhereYouAtMobile.Models;

namespace WhereYouAtMobile.Messages
{
    public class LocationTickMessage
    {
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        private Location location;
        public Location Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        private DateTime retrievedAt;
        public DateTime RetrievedAt
        {
            get
            {
                return retrievedAt;
            }

            set
            {
                retrievedAt = value;
            }
        }

    }
}
