using Common;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcAgent;
using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        private static GrpcChannel channel;
        private static Publisher.PublisherClient client;
        private static string category, location, id;


        static void Main(string[] args)
        {
            Console.WriteLine("Publisher");

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            channel = GrpcChannel.ForAddress(Constants.BrokerAddress);
            client = new Publisher.PublisherClient(channel);

            Console.Write("Enter category: ");
            category = Console.ReadLine().ToLower();
            Console.Write("Enter location: ");
            location = Console.ReadLine().ToLower();
            id = Guid.NewGuid().ToString();

            Timer t = new Timer(TimerCallback, null, 0, 2000);
            Console.ReadLine();
        }
        private static async void TimerCallback(Object o)
        {
            Random rand_data = new Random();
            var data = rand_data.Next(0, 10);
            var request = new PublishRequest() { Id= id, Category = category, Location = location, Data = data.ToString() };
            try
            {
                var reply = await client.PublishMessageAsync(request);
                Console.WriteLine($"Publish reply: {reply.IsSuccess}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error  {e.Message}");
            }
            GC.Collect();
        }
    }
}
