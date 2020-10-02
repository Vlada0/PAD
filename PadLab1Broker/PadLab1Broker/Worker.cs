using Newtonsoft.Json;
using PadLab1Broker.Model;
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
                        var connections = Storage.subscriberStorage.GetConnectionsByKeyWord(payload);
                        Console.WriteLine(connections.Count);
                        var newMessage = new NewMessage("newMessage", payload);
                        var message = JsonConvert.SerializeObject(newMessage);
                        Console.WriteLine(message);
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        foreach (var connection in connections)
                        {
                            connection.Socket.Send(data);
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}
