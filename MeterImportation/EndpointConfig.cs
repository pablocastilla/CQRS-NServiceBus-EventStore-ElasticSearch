
namespace MeterImportation
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
            /*var meterId = System.Guid.NewGuid();



            Parallel.For(0, 999999, i =>
            {
                var guidString = "26de36b7-76f5-4f17-8f9d-44eb50"+String.Format("{0:000000}", i);

                Bus.Send("MeterImportation",
                    new ImportMeterCommand()
                    {
                        MessageId = System.Guid.NewGuid(),
                        MeterId = new System.Guid(guidString),
                        SerialNumber = "Meter" + i
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
