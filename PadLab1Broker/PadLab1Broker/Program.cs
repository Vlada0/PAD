using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    class Program
    {
        static void Main(string[] args)
        {

            BrSocket brokerSocket = new BrSocket();
            brokerSocket.StartBroker("127.0.0.1", 11000);
            Console.ReadLine();
        }
    }
}
