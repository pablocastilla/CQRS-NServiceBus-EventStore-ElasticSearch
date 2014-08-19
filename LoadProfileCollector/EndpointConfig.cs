
namespace LoadProfileCollector
{
    using System;
    using System.Threading.Tasks;
    using Messages;
    using Messages.Commands;
    using NServiceBus;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,AsA_Publisher
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
        /*    var meterId = System.Guid.NewGuid();

            Parallel.For(0, 1000, i => Bus.Send("LoadProfileCollector", new ImportMeterCommand() 
                        {
                            MessageId = System.Guid.NewGuid(),
                            MeterId = System.Guid.NewGuid(),
                            SerialNumber = "Meter"+i

                        }));*/

          


            
        }

        public void Stop()
        {
           
        }
    }
}
