using PADLab1Part2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Services.Interfaces
{
    public interface IConnectionStorageService
    {
        void Add(Connection connection);

        void Subscribe(string keyword, string address);

        void Remove(string address);

        void Remove(string keyword, string address);

        IList<Connection> GetConnections(Message message);

        void Subscribe(string[] keywords, string address);
    }
}
