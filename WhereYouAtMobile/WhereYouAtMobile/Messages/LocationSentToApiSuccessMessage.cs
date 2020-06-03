using System;
using System.Collections.Generic;
using System.Text;

namespace WhereYouAtMobile.Messages
{
    class LocationSentToApiSuccessMessage
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
