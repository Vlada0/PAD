using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PadLab1Broker
{
    class Handler
    {
        public static void Handle(byte[] messageBytes, ConnectInformation connectionInfo)
        {
            int statusCode;
            var message = Encoding.UTF8.GetString(messageBytes);

            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);
            var operation = (string)json["operation"];
            switch (operation)
            {
                case "publish":
                    statusCode = 200;
                    Console.WriteLine(json["operationInfo"].ToString());
                    var payload = JsonConvert.DeserializeObject<Payload>(json["operationInfo"].ToString());
                    PayloadStorage.Add(payload);
                    //Console.WriteLine(payload);
                    break;
                case "subscribe":
                    statusCode = HandleSubscriber(connectionInfo, json["operationInfo"].ToString());
                    break;
                case "unsubscribe":
                    statusCode = HandleUnsubscribe(connectionInfo, json["operationInfo"].ToString());
                    break;
                case "subscribeDevice":
                    Console.WriteLine("Подписался");
                    statusCode = HandleSubscribeDevice(connectionInfo, json["operationInfo"].ToString());
                    break;
                case "registerDevice":
                    statusCode = RegisterPublisher(connectionInfo, json["operationInfo"].ToString());
                    break;
                default:
                    statusCode = 404;
                    break;
            }
            Response resp = new Response(statusCode);
            var jsonResponse = JsonConvert.SerializeObject(resp);
            var data = Encoding.UTF8.GetBytes(jsonResponse);
            connectionInfo.Socket.Send(data);
        }
       
        private static int RegisterPublisher(ConnectInformation connectionInfo, string message)
        {
            var registerDeviceData = JsonConvert.DeserializeObject<RegisterDeviceData>(message);
            var publisher = new PublisherInfo(connectionInfo, registerDeviceData.id);
            if (Storage.publisherStorage.isValidDevice(publisher.Id))
            {
                Storage.publisherStorage.Add(publisher);
                return 200;  
            }
            return 400;
        }
        private static int HandleSubscriber(ConnectInformation connectionInfo, string message)
        {
            var subscribeData = JsonConvert.DeserializeObject<SubscribeData>(message);
            var keyWord = subscribeData.keyWord;
            var subscriber = Storage.subscriberStorage.Contains(connectionInfo.Socket.RemoteEndPoint.ToString());
            if (subscriber == null)
            {
                subscriber = new SubscriberInfo(connectionInfo);
                Storage.subscriberStorage.Add(subscriber);
            }
            var statusCode = subscriber.Add(keyWord);
            
            return statusCode;
        }

        private static int HandleUnsubscribe(ConnectInformation connectionInfo, string message)
        {
            var unsubscribeData = JsonConvert.DeserializeObject<UnsubscribeData>(message);
            var keyWord = unsubscribeData.keyWord; 
            var subscriber = Storage.subscriberStorage.Contains(connectionInfo.Socket.RemoteEndPoint.ToString());
            if (subscriber != null)
            {
                return subscriber.Remove(keyWord);
            }
            return 402;
        }

        private static int HandleSubscribeDevice(ConnectInformation connectionInfo, string message)
        {
            Console.WriteLine(message);
            var subscribeDeviceData = JsonConvert.DeserializeObject<SubscribeDeviceData>(message);
            var subscriber = new SubscriberInfo(connectionInfo);
            Storage.subscriberStorage.Add(subscriber);

            string[] keyWords = new string[] { subscribeDeviceData.location, subscribeDeviceData.category };
            //var subscriber = Storage.subscriberStorage.Contains(connectionInfo.Socket.RemoteEndPoint.ToString());
            var statusCode = subscriber.SubscribeDevice(keyWords);
            return statusCode;
        }   
    }
}

