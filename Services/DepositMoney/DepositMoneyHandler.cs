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

namespace DepositMoney
{
    public class DepositMoneyHandler : IHandleMessages<DepositMoneyCommand>
    {
        public void Handle(DepositMoneyCommand message)
        {
            var domainRepository = ObjectFactory.GetInstance<IDomainRepository>();

            var client = domainRepository.GetById<Client>(message.ClientID);

            client.Deposit(message.Quantity, DateTime.UtcNow, message.TransactionId,message.FromATM);

            domainRepository.Save<Client>(client);
        }
    }
}
