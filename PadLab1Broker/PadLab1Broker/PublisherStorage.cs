using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class PublisherStorage : ConnectStorage<PublisherInfo>
    {
        public bool isValidUserName(string userName)
        {
            int count;
            lock (locker)
            {
                count = connections.Where(x => x.UserName == userName).ToList().Count;
            }
            return count==0;
        }

        public string GetUserByAddress(string address)
        {
            string userName;
            lock (locker)
            {
                userName = connections.Where(x => x.Socket.RemoteEndPoint.ToString() == address).ToList().LastOrDefault().UserName;
            }
            return userName;

        }
    }
}
