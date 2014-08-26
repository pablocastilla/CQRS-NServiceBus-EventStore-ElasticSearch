using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Repository;
using Domain.Aggregates;
using Messages.Commands;
using NServiceBus;

namespace CreateClient
{
    public class CreateClientHandler : IHandleMessages <CreateClientCommand>
    {
        public void Handle(CreateClientCommand message)
        {
            var connection = Configuration.CreateConnection();
            var domainRepository = new EventStoreDomainRepository(connection);


            try 
	        {	        
		        var client = domainRepository.GetById<Client>(message.ID);


	        }
	        catch (AggregateNotFoundException)
	        {

                try
                {
                    var client = Client.CreateClient(message.ID, message.Name);
                    client.Deposit(message.InitialDeposit, DateTime.UtcNow);

                    domainRepository.Save<Client>(client);

                }
                catch (Exception)
                {
                    
                 
                }
        		
	        }
           
        }
    }
}
