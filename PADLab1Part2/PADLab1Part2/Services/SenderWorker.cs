using Google.Protobuf;
using Grpc.Core;
using GrpcAgent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PADLab1Part2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PADLab1Part2.Services
{
    public class SenderWorker : IHostedService
    {
        private Timer timer;
        private const int TimeToWait = 2000;
        private readonly IMessageStorageService messageStorage;
        private readonly IConnectionStorageService connectionStorage;

        public SenderWorker(IServiceScopeFactory serviceScopeFactory)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                messageStorage = scope.ServiceProvider.GetRequiredService<IMessageStorageService>();
                connectionStorage = scope.ServiceProvider.GetRequiredService<IConnectionStorageService>();
            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoSendWork, null, 0, TimeToWait);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private void DoSendWork(object state)
        {
            while (!messageStorage.IsEmpty())
            {
                var message = messageStorage.GetNext();

                if (message != null)
                {
                    var connections = connectionStorage.GetConnections(message);

                    foreach(var connection in connections)
                    {
                        var client = new Notifier.NotifierClient(connection.Channel);
                        var request = new NotifyRequest() { Id = message.Id, Category = message.Category, Location = message.Location, Data = message.Data };
                        try
                        {
                            var reply = client.Notify(request);
                            Console.WriteLine($"Notified subscriber {connection.Address} with {message.Data}. Response: {reply.IsSuccess}");
                        }
                        catch(RpcException rpcException)
                        {
                            if(rpcException.StatusCode == StatusCode.Internal)
                            {
                                connectionStorage.Remove(connection.Address);
                            }
                            Console.WriteLine($"RPC Error {connection.Address} {rpcException.Message}");
                        }
                        catch(Exception exception)
                        {
                            Console.WriteLine($"Error notifying {connection.Address}. {exception.Message}");
                        } 
                    }
                }
            }
        }
    }
}
