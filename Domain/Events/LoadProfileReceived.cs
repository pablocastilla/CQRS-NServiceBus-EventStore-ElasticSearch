using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;

namespace Domain.Events
{
    public class LoadProfileReceived : IDomainEvent
    {
        public string SerialNumber { get; set; }

         public  List<LoadProfileRead> LPReads { get; set; }
    }
}
