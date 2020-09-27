using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PADLab1Part2.Models
{
    public class Message
    {
        public Message(string id, string category, string location, string data)
        {
            
            Id = id;
            Category = category;
            Location = location;
            Data = data;
        }
        public string Id { get; }
        public string Category { get; }
        public string Location { get; }
        public string Data { get; }
    }

}
