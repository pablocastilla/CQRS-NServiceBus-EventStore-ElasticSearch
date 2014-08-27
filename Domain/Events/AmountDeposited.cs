using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;

namespace Domain.Events
{
    public class AmountDeposited : IDomainEvent
    {
        public string ID { get; set; }

        public double Quantity { get; set; }

        public DateTime TimeStamp { get; set; }

        public AmountDeposited(double quantity, DateTime timeStamp, string id)
        {
            Quantity = quantity;

            TimeStamp = timeStamp;

            ID = id;
        }
    }
}
