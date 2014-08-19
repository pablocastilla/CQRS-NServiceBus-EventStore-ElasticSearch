using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;

namespace Domain.Events
{
    public class MeterCreated :  IDomainEvent
    {
        public Guid Id { get; set; }

        public string SerialNumber { get; set; }


        public MeterCreated(Guid id, string serialNumber)
        {
            this.Id = id;
            this.SerialNumber = serialNumber;
        }
    }
}
