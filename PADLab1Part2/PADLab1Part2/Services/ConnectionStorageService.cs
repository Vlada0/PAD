using PADLab1Part2.Models;
using PADLab1Part2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Services
{
    public class ConnectionStorageService : IConnectionStorageService
    {
        private readonly List<Connection> connections;
        private readonly object locker;

        public ConnectionStorageService()
        {
            connections = new List<Connection>();
            locker = new object();
        }
        public void Add(Connection connection)
        {
            lock (locker)
            {
                connections.Add(connection);
            }
        }

        public IList<Connection> GetConnections(Message message)
        {
            lock (locker)
            {
                var filteredConnections = connections.Where(x =>x.isDevice ? x.keyWordList.Contains(message.Category)&& x.keyWordList.Contains(message.Location):
                x.keyWordList.Contains(message.Category) || x.keyWordList.Contains(message.Location)).ToList();
                return filteredConnections;
            }
        }

        public void Remove(string address)
        {
            lock(locker)
            {
                connections.RemoveAll(x => x.Address == address);
            }
        }

        public void Remove(string keyWord, string address)
        {
            lock (locker)
            {
                var connection = connections.Where(x => x.Address == address).ToList().FirstOrDefault();
                if (connection!=null)
                {
                    connection.keyWordList.Remove(keyWord);
                }
            }
        }

        public void Subscribe(string keyWord, string address)
        {
            lock (locker)
            {
                var connection = connections.Where(x => x.Address == address).ToList().FirstOrDefault();
                if (connection != null)
                {
                    connection.keyWordList.Add(keyWord);
                }
            }
        }

        public void Subscribe(string[] keywords, string address)
        {
            lock (locker)
            {
                var connection = connections.Where(x => x.Address == address).ToList().FirstOrDefault();
                if (connection != null)
                {
                    connection.keyWordList.AddRange(keywords);
                }
            }
        }
    }
}
