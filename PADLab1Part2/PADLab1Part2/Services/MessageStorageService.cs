using PADLab1Part2.Models;
using PADLab1Part2.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Services
{
    public class MessageStorageService : IMessageStorageService
    {
        private readonly ConcurrentQueue<Message> messages;

        public MessageStorageService()
        {
            messages = new ConcurrentQueue<Message>();
        }
        public void Add(Message message)
        {
            messages.Enqueue(message);
        }

        public Message GetNext()
        {
            Message message;
            messages.TryDequeue(out message);
            return message;
        }

        public bool IsEmpty()
        {
            return messages.IsEmpty;
        }
    }
}
