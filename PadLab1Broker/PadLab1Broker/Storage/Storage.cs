using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public static class Storage
    {
        public static PublisherStorage publisherStorage { get; set; }
        public static SubscribersStorage subscriberStorage { get; set; }

        static Storage()
        {
            publisherStorage = new PublisherStorage();
            subscriberStorage = new SubscribersStorage();
        }
    }
}
