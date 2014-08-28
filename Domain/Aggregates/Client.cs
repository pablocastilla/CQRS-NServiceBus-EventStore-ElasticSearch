using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.DomainBase;
using Domain.Events;

namespace Domain.Aggregates
{
    public class Client : AggregateBase,IAggregate
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public double balance { get; set; }

        public override string AggregateId
        {
            get { return ID; }
        }


        public Client()
        {
            RegisterTransition<ClientCreated>(Apply);
            RegisterTransition<AmountDeposited>(Apply);
        }

        private void Apply(AmountDeposited obj)
        {
            balance += obj.Quantity;
            
        }

        private void Apply(ClientCreated obj)
        {
            this.ID = obj.ID;

            Name = obj.Name;

            balance = 0;
        }


        public Client(string id, string name) : this()
        {
            RaiseEvent(new ClientCreated(id,name));
        }

        public static Client CreateClient(string id, string name)
        {
            return new Client(id, name);
        }

        public void Deposit(double quantity, DateTime timeStamp, Guid transactionId)
        {
            RaiseEvent(new AmountDeposited(quantity, timeStamp, ID, transactionId));
        }
    }
}
