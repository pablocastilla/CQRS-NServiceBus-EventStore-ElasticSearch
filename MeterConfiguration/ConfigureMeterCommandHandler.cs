using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Repository;
using Domain;
using Messages.Commands;
using NServiceBus;

namespace MeterConfiguration
{
    public class ConfigureMeterCommandHandler : IHandleMessages<ConfigureMeterCommand>
    {
        public IBus Bus { get; set; }

        public void Handle(ConfigureMeterCommand message)
        {
            //some magic stuff with the meter that ends in an operative meter that sends it first read

            var connection = Configuration.CreateConnection();
            var domainRepository = new EventStoreDomainRepository(connection);

            var meter = domainRepository.GetById<Meter>(message.SerialNumber);

            meter.ChangeMeterState(Meter.MeterState.Operative);

            domainRepository.Save<Meter>(meter);


          
        }
    }
}
