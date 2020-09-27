using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    class Response
    {
        public int statusCode { get; set; }

        public Response(int status_Code)
        {
            this.statusCode = status_Code;
        }
    }

    class SubscribeData
    {
        public string keyWord { get; set; }
    }

    class UnsubscribeData
    {
        public string keyWord { get; set; }
    }
    class SubscribeDeviceData
    {
        public string category { get; set; }
        public string location { get; set; }
    }
    class RegisterDeviceData
    {
        public string id { get; set; }
    }
}
