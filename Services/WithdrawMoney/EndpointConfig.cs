
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
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();


            ObjectFactory.Initialize(o => o.For<IDomainRepository>().Use<EventStoreDomainRepository>());
        }
    }
}
