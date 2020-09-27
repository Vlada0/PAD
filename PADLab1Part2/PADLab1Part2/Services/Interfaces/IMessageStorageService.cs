using PADLab1Part2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Services.Interfaces
{
    public interface IMessageStorageService
    {
        void Add(Message message);

        Message GetNext();

        bool IsEmpty();
    }
}
