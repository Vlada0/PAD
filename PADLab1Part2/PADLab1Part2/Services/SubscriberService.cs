using Grpc.Core;
using GrpcAgent;
using PADLab1Part2.Models;
using PADLab1Part2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Services
{
    public class SubscriberService : Subscriber.SubscriberBase
    {
        private readonly IConnectionStorageService connectionStorage;
        public SubscriberService(IConnectionStorageService _connectionStorage)
        {
            connectionStorage = _connectionStorage;
        }
        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {
            Console.WriteLine($"New client trying to subscribe {request.Address} {request.KeyWord}");
            try
            {
                connectionStorage.Subscribe(request.KeyWord, request.Address);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not add new connection {request.Address} {request.KeyWord} {e.Message}");
            }
            
            return Task.FromResult(new SubscribeReply()
            {
                IsSuccess = true
            });
        }

        public override Task<SubscribeReply> DeviceSubscribe(DeviceSubscribeRequest request, ServerCallContext context)
        {
            Console.WriteLine($"New client trying to subscribe {request.Address} {request.Category} {request.Category}");
            try
            {
                string[] keyWords = new string[] { request.Category, request.Location };
                connectionStorage.Subscribe(keyWords, request.Address);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not subscribe {request.Address} {request.Location} {request.Category} {e.Message}");
            }

            return Task.FromResult(new SubscribeReply()
            {
                IsSuccess = true
            });
        }

        public override Task<SubscribeReply> Unsubscribe(UnsubscribeRequest request, ServerCallContext context)
        {
            Console.WriteLine($"New client trying to unsubscribe {request.Address} {request.KeyWord}");
            try
            {
                connectionStorage.Remove(request.KeyWord, request.Address);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not unsubscribe {request.Address} {request.KeyWord} {e.Message}");
            }

            return Task.FromResult(new SubscribeReply()
            {
                IsSuccess = true
            });
        }

        public override Task<SubscribeReply> Connect (ConnectRequest request, ServerCallContext context)
        {
            try
            {
                var connection = new Connection(request.Address, request.IsDevice);
                connectionStorage.Add(connection);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not connect {request.Address} {e.Message}");
            }

            return Task.FromResult(new SubscribeReply()
            {
                IsSuccess = true
            });
        }

    }
}
