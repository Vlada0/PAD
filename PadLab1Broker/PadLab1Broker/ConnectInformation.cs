using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public class ConnectInformation
    {
        public const int Buffer_Size = 2048;

        public byte[] Buffer { get; set; }

        public Socket Socket { get; set; }
       // public string Address { get; set; }
       // public string Topic { get; set; }
       // public string UserName { get; set; }


        public ConnectInformation()
        {
            Buffer = new byte[Buffer_Size];
        }
    }
}
