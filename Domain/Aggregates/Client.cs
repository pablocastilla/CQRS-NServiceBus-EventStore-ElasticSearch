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
            RegisterTransition<MoneyDeposited>(Apply);
            RegisterTransition<MoneyWithdrawn>(Apply);
        }

        private void Apply(ClientCreated obj)
        {
            this.ID = obj.ID;

            Name = obj.Name;

            balance = 0;
        }

        private void Apply(MoneyDeposited obj)
        {
            balance += obj.Quantity;
            
        }             

        private void Apply(MoneyWithdrawn obj)
        {
            balance -= obj.Quantity;
        }


        public Client(string id, string name) : this()
        {
            RaiseEvent(new ClientCreated(id,name));
        }

        public static Client CreateClient(string id, string name)
        {
            return new Client(id, name);
        }

        public void Deposit(double quantity, DateTime timeStamp, Guid transactionId, bool fromATM = false)
        {
            RaiseEvent(new MoneyDeposited(quantity, timeStamp, ID, transactionId, fromATM));
        }

        public void Withdraw(double quantity, DateTime timeStamp, Guid transactionId, bool fromATM = false)
        {
            RaiseEvent(new MoneyWithdrawn(quantity, timeStamp, ID, transactionId, fromATM));
        }
    }
}
