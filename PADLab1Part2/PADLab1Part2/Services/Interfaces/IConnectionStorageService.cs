using PADLab1Part2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Services.Interfaces
{
    public interface IConnectionStorageService
    {
        int Add(Connection connection);

        int Subscribe(string keyword, string address);

        void Remove(string address);

        int Remove(string keyword, string address);

        IList<Connection> GetConnections(Message message);

        int Subscribe(string[] keywords, string address);
    }
}
