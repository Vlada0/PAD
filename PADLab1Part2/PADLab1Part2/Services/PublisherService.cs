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
    public class PublisherService :Publisher.PublisherBase
    {
        private readonly IMessageStorageService messageStorage;
        public PublisherService(IMessageStorageService messageStorageService)
        {
            messageStorage = messageStorageService;
        }
        public override Task<PublishReply> PublishMessage(PublishRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Received: {request.Category} {request.Location} {request.Data}");
            var message = new Message(request.Id, request.Category, request.Location, request.Data);
            messageStorage.Add(message);

            return Task.FromResult(new PublishReply()
            {
                IsSuccess = true
            });
        }
    }
}
