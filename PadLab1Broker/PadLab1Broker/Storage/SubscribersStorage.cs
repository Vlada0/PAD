using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class SubscribersStorage: ConnectStorage<SubscriberInfo>
    {
        
        public List<SubscriberInfo> GetConnectionsByKeyWord(Payload message)
        {
            List<SubscriberInfo> selected;
            lock (locker)
            {
                selected = connections.Where(x => x.isDevice ? (x.keyWords.Contains(message.category) && x.keyWords.Contains(message.location)):
                (x.keyWords.Contains(message.category) || x.keyWords.Contains(message.location))).ToList();
                
                foreach(var connection in connections)
                {
                    Console.WriteLine(connection);
                }
               //connections.ForEach(x => Console.WriteLine($" { x.keyWords.FirstOrDefault()} {x.keyWords.LastOrDefault()}"));
                
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
