using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    class Handler
    {
        public static void Handle(byte[] messageBytes, ConnectInformation connectionInfo)
        {
            var message = Encoding.UTF8.GetString(messageBytes);
            Console.WriteLine(message);
        }
    }
}
