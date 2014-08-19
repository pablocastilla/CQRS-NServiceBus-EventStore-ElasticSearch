using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LoadProfileRead
    {
        public Guid Id { get; set; }

        public string SerialNumber { get; set; }

        public DateTime ReadTimeStamp { get; set; }

        public DateTime EntryDateTime { get; set; }

        public double Value { get; set; }
    }
}
