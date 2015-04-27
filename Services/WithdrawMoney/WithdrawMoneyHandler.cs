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
        IDomainRepository domainRepository;

        public WithdrawMoneyHandler( IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }


        public void Handle(WithdrawMoneyCommand message)
        {                       

            var client = domainRepository.GetById<Client>(message.ClientID);

            client.Withdraw(message.Quantity, DateTime.UtcNow, message.TransactionId, message.FromATM);

            domainRepository.Save<Client>(client);
        }
    }
}
