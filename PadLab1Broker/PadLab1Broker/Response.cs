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
}
