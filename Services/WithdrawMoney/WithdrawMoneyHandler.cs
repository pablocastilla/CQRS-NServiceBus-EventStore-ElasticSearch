using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Repository;
using Domain.Aggregates;
using Messages.Commands;
using NServiceBus;
using StructureMap;

namespace WithdrawMoney
{
    public class WithdrawMoneyHandler : IHandleMessages<WithdrawMoneyCommand>
    {
        public void Handle(WithdrawMoneyCommand message)
        {

            var domainRepository = ObjectFactory.GetInstance<IDomainRepository>();

            var client = domainRepository.GetById<Client>(message.ClientID);

            client.Withdraw(message.Quantity, DateTime.UtcNow, message.TransactionId, message.FromATM);

            domainRepository.Save<Client>(client);
        }
    }
}
