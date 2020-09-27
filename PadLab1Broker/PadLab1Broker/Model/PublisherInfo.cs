using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class PublisherInfo:ConnectInformation
    {
        public string Id { get; set; }
        public PublisherInfo(ConnectInformation connection, string id):base()
        {
            base.Socket = connection.Socket;
            base.Buffer = connection.Buffer;
            Id = id;
        }
        
    }
}
