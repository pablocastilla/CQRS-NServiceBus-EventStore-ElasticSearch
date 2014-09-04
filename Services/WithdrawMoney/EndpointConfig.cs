
namespace WithdrawMoney
{
    using System;
    using CrossCutting.Repository;
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
}
