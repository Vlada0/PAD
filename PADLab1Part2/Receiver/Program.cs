using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Grpc.Net.Client;
using GrpcAgent;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(Constants.SubscribersAddress)
                .Build();
            host.Start();

            var selection = ShowMenu();
            switch (selection)
            {
                case "1":
                    Connect(host, true);
                    Subscribe(host, "temperature", "kitchen");
                    break;
                case "2":
                    Connect(host, true);
                    Subscribe(host, "brightness", "bedroom");
                    break;
                case "3":
                    Connect(host, false);
                    UserStartFlow(host);
                    break;
                default:
                    break;
            }
        }

    private static void Connect(IWebHost host, bool isDevice)
        {
            var channel = GrpcChannel.ForAddress(Constants.BrokerAddress);
            var client = new Subscriber.SubscriberClient(channel);
            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            Console.WriteLine($"Connect to: {address}");
            var request = new ConnectRequest() {Address = address, IsDevice = isDevice };

            try
            {
                var reply = client.Connect(request);
                Console.WriteLine($"Reply: {reply.StatusCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error connecting");
            }
        }
        private static string ShowMenu()
        {
            Console.WriteLine("Select device: ");
            Console.WriteLine("1. Smart Window");
            Console.WriteLine("2. Lamp");
            Console.WriteLine("3. CellPhone");
            var selection = Console.ReadLine().ToString();
            if(selection == "1"|| selection == "2"||selection=="3")
            {
                return selection;
            }
            Console.WriteLine("Wrong selection! Try again!");
            return ShowMenu();
        }

        private static void UserStartFlow(IWebHost host)
        {
            while (true)
            {
                Console.WriteLine("Select option: ");
                Console.WriteLine("1. Subscribe");
                Console.WriteLine("2. Unsubscribe");
                var selection = Console.ReadLine().ToString();
                switch (selection)
                {
                    case "1":
                        Subscribe(host);
                        break;
                    case "2":
                        Unsubscribe(host);
                        break;
                    default:
                        return;
                }

            }
            
        }

        private static void Subscribe(IWebHost host)
        {
            var channel = GrpcChannel.ForAddress(Constants.BrokerAddress);
            var client = new Subscriber.SubscriberClient(channel);

            Console.Write("Enter location or category: ");
            var keyWord = Console.ReadLine().ToLower();

            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            Console.WriteLine($"Subscriber listening at: {address}");
            var request = new SubscribeRequest() { KeyWord = keyWord, Address = address };

            try
            {
                var reply = client.Subscribe(request);
                Console.WriteLine($"Reply: {reply.StatusCode}");
                if (reply.StatusCode != 200)
                {
                    Console.WriteLine("Cannot subscribe");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error subscribing: {e.Message}");
            }
        }


        private static void Subscribe(IWebHost host, string category, string location)
        {
            var channel = GrpcChannel.ForAddress(Constants.BrokerAddress);
            var client = new Subscriber.SubscriberClient(channel);

           // Console.Write("Enter location or category: ");
            //var keyWord = Console.ReadLine().ToLower();

            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            Console.WriteLine($"Subscriber listening at: {address}");
            var request = new DeviceSubscribeRequest() { Category = category, Location = location, Address = address };

            try
            {
                var reply = client.DeviceSubscribe(request);
                Console.WriteLine($"Reply: {reply.StatusCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error subscribing");
            }
            Console.ReadLine();
        }
        private static void Unsubscribe(IWebHost host)
        {
            var channel = GrpcChannel.ForAddress(Constants.BrokerAddress);
            var client = new Subscriber.SubscriberClient(channel);

            Console.Write("Remove: ");
            var keyWord = Console.ReadLine().ToLower();

            var address = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
           // Console.WriteLine($"Subscriber listening at: {address}");
            var request = new UnsubscribeRequest() { KeyWord = keyWord, Address = address };

            try
            {
                var reply = client.Unsubscribe(request);
                Console.WriteLine($"Reply: {reply.StatusCode}");
                if (reply.StatusCode != 200)
                {
                    Console.WriteLine("Cannot unsubscribe");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error subscribing");
            }
        }
    }
}
