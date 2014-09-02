using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;

namespace Domain.Events
{
    public class MoneyWithdrawn : IDomainEvent
    {
         public Guid TransactionId { get; set; }

        public string ClientID { get; set; }

        public double Quantity { get; set; }

        public DateTime TimeStamp { get; set; }

        public bool FromATM { get; set; }

        public MoneyWithdrawn(double quantity, DateTime timeStamp, string userId, Guid transactionId, bool fromATM)
        {
            Quantity = quantity;

            TimeStamp = timeStamp;

            ClientID = userId;

            TransactionId = transactionId;

            FromATM = fromATM;
        }
    }
}
