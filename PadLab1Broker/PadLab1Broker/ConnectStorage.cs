using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadLab1Broker
{
    public abstract class ConnectStorage<T> where T: ConnectInformation
    {
        
        protected  List<T> connections;
        protected  object locker;

        public ConnectStorage()
        {
            connections = new List<T>();
            locker = new object();
        }

        public void Add(T connection)
        {
            lock (locker)
            {
                connections.Add(connection);
            }
        }

        public void Remove(string address)
        {
            lock (locker)
            {
                connections.RemoveAll(x => x.Socket.RemoteEndPoint.ToString() == address);
            }
        }

     
    }
}
