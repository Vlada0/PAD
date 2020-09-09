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
            int statusCode;
            var message = Encoding.UTF8.GetString(messageBytes);
            string publisherName = "{\"publisherName\":";
            string subscribe = "{\"subscribe\":";
            string unsubscribe = "{\"unsubscribe\":";

            if (message.StartsWith(publisherName))
            {
                statusCode = HandleNewPublisher(connectionInfo, message);
            }
            else if (message.StartsWith(subscribe))
            {
                statusCode = HandleSubscriber(connectionInfo, message);
            }
            else if (message.StartsWith(unsubscribe))
            {
               statusCode = HandleUnsubscribe(connectionInfo, message);
            }
            else
            {
                statusCode = 200;
                Payload payload = JsonConvert.DeserializeObject<Payload>(message);
                payload.username = Storage.publisherStorage.GetUserByAddress(connectionInfo.Socket.RemoteEndPoint.ToString());
                PayloadStorage.Add(payload);
            }

            Response resp = new Response(statusCode);
            var json = JsonConvert.SerializeObject(resp);
            var data = Encoding.UTF8.GetBytes(json);
            connectionInfo.Socket.Send(data);
        }

        private static int HandleNewPublisher(ConnectInformation connectionInfo, string message)
        {
            var userNameResponse = JsonConvert.DeserializeObject<UserNameResponse>(message);
            var userName = userNameResponse.publisherName;
            var publisher = new PublisherInfo(connectionInfo, userName);
            if (Storage.publisherStorage.isValidUserName(userName))
            {
                Storage.publisherStorage.Add(publisher);
                return 200;//OK
            }
            return 400; //
            
        }

        private static int HandleSubscriber(ConnectInformation connectionInfo, string message)
        {
            var subscribeResponse = JsonConvert.DeserializeObject<SubscribeResponse>(message);
            var topic = subscribeResponse.subscribe;
            var subscriber = Storage.subscriberStorage.Contains(connectionInfo.Socket.RemoteEndPoint.ToString());
            if (subscriber == null)
            {
                subscriber = new SubscriberInfo(connectionInfo);
                Storage.subscriberStorage.Add(subscriber);
            }
            var statusCode = subscriber.Add(topic);
            return statusCode;
        }

        private static int HandleUnsubscribe(ConnectInformation connectionInfo, string message)
        {

            var unsubscribeResponse = JsonConvert.DeserializeObject<UnsubscribeResponse>(message);
            var topic = unsubscribeResponse.unsubscribe; 
            var subscriber = Storage.subscriberStorage.Contains(connectionInfo.Socket.RemoteEndPoint.ToString());
            var statusCode = subscriber.RemoveTopic(topic);
            return statusCode;
        }
    }
}
