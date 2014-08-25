using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;

namespace Domain.Events
{
    public class ClientCreated : IDomainEvent
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public ClientCreated(string id, string name)
        {
            ID = id;

            Name = name;
        }

    }
}
