using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class SubscriberInfo: ConnectInformation
    {
        public List<string> topicList { get; set; }

        public SubscriberInfo(ConnectInformation connection):base()
        {
            base.Socket = connection.Socket;
            base.Buffer = connection.Buffer;
            topicList = new List<string>();
        }

        public int Add(string topic)
        {
            if (topicList.Contains(topic))
            {
                return 401;//Topic already exists
            }
            topicList.Add(topic);
            return 200;
        }
        public int RemoveTopic(string topic)
        {
            return topicList.Remove(topic)?200:402;//402-Topic does not exist (не подписан)
        }
    }
}
