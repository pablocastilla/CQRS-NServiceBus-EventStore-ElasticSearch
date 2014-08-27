
namespace CreateClient
{
    using System;
    using System.Threading.Tasks;
    using Messages.Commands;
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, AsA_Publisher
    {
        public void Init()
        {

            Configure.Transactions.Advanced(settings =>
            {
                settings.DisableDistributedTransactions();
                settings.DefaultTimeout(TimeSpan.FromSeconds(120));
            });
        }
    }

    public class MyClass : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {


           /* Parallel.For(0, 10000, i =>
            {


                Bus.Send("CreateClient",
                    new CreateClientCommand()
                    {
                        MessageId = System.Guid.NewGuid(),
                        TransactionId = System.Guid.NewGuid(),
                        ID=i.ToString(),
                        Name = "Client_" + i,
                        InitialDeposit= 1000
                    }
                );
            }
            );*/

        }

        public void Stop()
        {

        }
    }


}
