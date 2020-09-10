using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    class Worker
    {
        public void DoSendMessageWork()
        {
            while (true)
            {
                while (!PayloadStorage.isEmpty())
                {
                    var payload = PayloadStorage.GetNext();

                    if (payload != null)
                    {
                        var connections = Storage.subscriberStorage.GetConnectionsByTopic(payload.topic);

                        foreach(var connection in connections)
                        {
                            var message = JsonConvert.SerializeObject(payload);
                            byte[] data = Encoding.UTF8.GetBytes(message);

                            connection.Socket.Send(data);
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}
