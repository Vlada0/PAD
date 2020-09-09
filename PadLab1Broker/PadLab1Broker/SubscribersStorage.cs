using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class SubscribersStorage: ConnectStorage<SubscriberInfo>
    {
        
        public List<SubscriberInfo> GetConnectionsByTopic(string topic)
        {
            List<SubscriberInfo> selected;
            lock (locker)
            {
                selected = connections.Where(x => x.topicList.Contains(topic)).ToList();
            }
            return selected;
        }

        public SubscriberInfo Contains(string remoteAddress)
        {
            SubscriberInfo subscriber;
            lock (locker)
            {
                subscriber = connections.Where(x => x.Socket.RemoteEndPoint.ToString() == remoteAddress).ToList().LastOrDefault();
            }
            return subscriber;
        }
    }
}
