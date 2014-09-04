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

namespace CreateClient
{
    public class CreateClientHandler : IHandleMessages <CreateClientCommand>
    {
        public void Handle(CreateClientCommand message)
        {
            var domainRepository = ObjectFactory.GetInstance<IDomainRepository>();

                     
            var client = Client.CreateClient(message.ClientID, message.Name);
            client.Deposit(message.InitialDeposit, DateTime.UtcNow,message.TransactionId);

            domainRepository.Save<Client>(client, true);

                      
        }
    }
}
