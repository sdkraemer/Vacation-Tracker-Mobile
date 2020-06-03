using System;
using System.Collections.Generic;
using System.Text;

namespace WhereYouAtMobile.Messages
{
    class LocationReceivedMessage
    {
        private DateTime receivedAt;
        public DateTime ReceivedAt
        {
            get
            {
                return receivedAt;
            }

            set
            {
                receivedAt = value;
            }
        }
    }
}
