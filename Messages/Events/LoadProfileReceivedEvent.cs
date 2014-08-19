using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class LoadProfileReceivedEvent : BusMessage, IEvent
    {
        public Guid MeterId { get; set; }

        public string SerialNumber { get; set; }

        public DateTime ReadTimeStamp { get; set; }

        public DateTime EntryDateTime { get; set; }

        public double Value { get; set; }
    }
}
