using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    static class PayloadStorage
    {
        private static ConcurrentQueue<Payload> payloadQueue;

        static PayloadStorage()
        {
            payloadQueue = new ConcurrentQueue<Payload>();
        }

        public static void Add(Payload payload)
        {
            payloadQueue.Enqueue(payload);
        }

        public static Payload GetNext()
        {
            Payload payload = null;
            payloadQueue.TryDequeue(out payload);
            return payload;
        }

        public static bool isEmpty()
        {
            return payloadQueue.IsEmpty;
        }
    }
}
