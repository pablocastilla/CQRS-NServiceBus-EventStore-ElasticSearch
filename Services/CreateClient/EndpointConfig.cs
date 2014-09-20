
namespace CreateClient
{
    using System;
    using System.Threading.Tasks;
    using CrossCutting.Repository;
    using Messages.Commands;
    using NServiceBus;
    using StructureMap;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.Transactions.Advanced(settings =>
            {
                settings.DisableDistributedTransactions();
                settings.DefaultTimeout(TimeSpan.FromSeconds(120));
            });


            ObjectFactory.Initialize(o => o.For<IDomainRepository>().Use<EventStoreDomainRepository>());
        }
    }

    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {

            Parallel.For(30000, 45000, i =>
            {
                Bus.Send("CreateClient",
                    new CreateClientCommand()
                    {
                        MessageId = System.Guid.NewGuid(),
                        TransactionId = System.Guid.NewGuid(),
                        ClientID=i.ToString(),
                        Name = "Client_" + i,
                        InitialDeposit= 1000
                    }
                );

               /* Bus.Send("DepositMoney",
                    new DepositMoneyCommand()
                    {
                        MessageId = System.Guid.NewGuid(),
                        TransactionId = System.Guid.NewGuid(),
                        ClientID = i.ToString(),                       
                        Quantity = 10
                    }
                );

                Bus.Send("WithdrawMoney",
                   new WithdrawMoneyCommand()
                   {
                       MessageId = System.Guid.NewGuid(),
                       TransactionId = System.Guid.NewGuid(),
                       ClientID = i.ToString(),                     
                       Quantity = 10
                   }
               );


                Bus.Send("WithdrawMoney",
                   new WithdrawMoneyCommand()
                   {
                       MessageId = System.Guid.NewGuid(),
                       TransactionId = System.Guid.NewGuid(),
                       ClientID = i.ToString(),
                       Quantity = 10
                   }
               );

                Bus.Send("WithdrawMoney",
                   new WithdrawMoneyCommand()
                   {
                       MessageId = System.Guid.NewGuid(),
                       TransactionId = System.Guid.NewGuid(),
                       ClientID = i.ToString(),
                       Quantity = 10
                   }
               );*/
            }
            );

        }

        public void Stop()
        {

        }
    }


}
