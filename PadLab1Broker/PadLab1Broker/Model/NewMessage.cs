using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker.Model
{
    public class NewMessage
    {
        public string operation { get; set; }
        public Payload operationInfo { get; set; }

        public NewMessage(string Operation, Payload OperationInfo)
        {
            operation = Operation;
            operationInfo = OperationInfo;
        }
    }
}
