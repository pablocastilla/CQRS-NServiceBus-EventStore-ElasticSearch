
namespace DepositMoney
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
            var container = new Container();

            container.Configure(
               cfg =>
               {
                   cfg.Policies.FillAllPropertiesOfType<IContainer>();
                   cfg.For<IDomainRepository>().Use<EventStoreDomainRepository>();

               }
               );

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.Transactions().DisableDistributedTransactions();
            configuration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));
        }
    }
}
