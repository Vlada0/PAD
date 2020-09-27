using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class PublisherStorage : ConnectStorage<PublisherInfo>
    {
        public bool isValidDevice(string id)
        {
            int count;
            lock (locker)
            {
                count = connections.Where(x => x.Id == id).ToList().Count;
            }
            return count==0;
        }

        public string GetUserByAddress(string address)
        {
            string id = "";

            lock (locker)
            {
                var user = connections.Where(x => x.Socket.RemoteEndPoint.ToString() == address).ToList().LastOrDefault();
                if(user != null)
                {
                    id = user.Id;
                }
            }
            return id;

        }
    }
}
