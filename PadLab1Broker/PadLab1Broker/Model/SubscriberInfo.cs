using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class SubscriberInfo: ConnectInformation
    {
        public bool isDevice = false;
        public List<string> keyWords { get; set; }

        public SubscriberInfo(ConnectInformation connection):base()
        {
            base.Socket = connection.Socket;
            base.Buffer = connection.Buffer;
            keyWords = new List<string>();
        }

        public int Add(string keyWord)
        {
            if (keyWords.Contains(keyWord))
            {
                return 401;//Key already exists
            }
            keyWords.Add(keyWord);
            return 200;
        }

        public int Remove(string keyWord)
        {
            return keyWords.Remove(keyWord)?200:402;//402-Keyword does not exist (не подписан)
        }

        public int SubscribeDevice(string[] keywords)
        {
            isDevice = true;
            keyWords.AddRange(keywords);
            return 200;
        }

    }
}
