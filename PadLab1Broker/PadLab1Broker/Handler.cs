using Newtonsoft.Json;
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
            string publisherName = "{\"publisherName\":";
            string subscribe = "{\"subscribe\":";

            if (message.StartsWith(publisherName))
            {
                var userNameResponse = JsonConvert.DeserializeObject<UserNameResponse>(message);
                var userName = userNameResponse.publisherName; //message.Split(publisherNameArray).LastOrDefault();
                var publisher = new PublisherInfo(connectionInfo, userName);
                Response resp;
                if (Storage.publisherStorage.isValidUserName(userName))
                {
                    Storage.publisherStorage.Add(publisher);
                    resp = new Response(200);
                }
                else
                {
                    resp = new Response(400);
                }
                var json = JsonConvert.SerializeObject(resp);
                var data = Encoding.UTF8.GetBytes(json);
                connectionInfo.Socket.Send(data);  
            }
            else if (message.StartsWith(subscribe))
            {
                var subscribeResponse = JsonConvert.DeserializeObject<SubscribeResponse>(message);
                var topic = subscribeResponse.subscribe; //message.Split(publisherNameArray).LastOrDefault();
                var subscriber = Storage.subscriberStorage.Contains(connectionInfo.Socket.RemoteEndPoint.ToString());
                if (subscriber==null)
                {
                    subscriber = new SubscriberInfo(connectionInfo);
                    Storage.subscriberStorage.Add(subscriber);
                }
                var statusCode = subscriber.Add(topic);
                Response resp = new Response(statusCode);
               
                var json = JsonConvert.SerializeObject(resp);
                var data = Encoding.UTF8.GetBytes(json);
                connectionInfo.Socket.Send(data);
            }
            else
            {
                Payload payload = JsonConvert.DeserializeObject<Payload>(message);
                PayloadStorage.Add(payload);
            }

            
        }
    }
}
