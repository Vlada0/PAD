using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Models
{
    public class Connection
    {
        public Connection(string address, bool isdevice)
        {
            Address = address;
            isDevice = isdevice;
            keyWordList = new List<string>();
            Channel = GrpcChannel.ForAddress(address);
        }
        public string Address { get; }
        public List<string> keyWordList { get; set; }
        public GrpcChannel Channel { get; }

        public bool isDevice { get; }

    }
}
