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

        public string SerialNumber { get; set; }

        public List<LoadProfileVO> LoadProfile { get; set; }
    
    }

    public class LoadProfileVO
    {
        public DateTime ReadTimeStamp { get; set; }

        public DateTime EntryDateTime { get; set; }

        public double Value { get; set; }
    }
}
