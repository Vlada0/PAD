using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class PublisherInfo:ConnectInformation
    {
        public string UserName { get; set; }


        public PublisherInfo(ConnectInformation connection, string userName):base()
        {
            base.Socket = connection.Socket;
            base.Buffer = connection.Buffer;
            UserName = userName;
        }
        
    }
}
