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
       
        public string SerialNumber { get; set; }


        public MeterCreated(string serialNumber)
        {
            
            this.SerialNumber = serialNumber;
        }

       
    }
}
