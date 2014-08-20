using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Repository;
using Domain;
using Domain.Events;
using Messages;
using Messages.Commands;
using NServiceBus;

namespace LoadProfileCollector
{
    public class ImportMeterCommandHandler : IHandleMessages<ImportMeterCommand>
    {
        public IBus Bus { get; set; }

        public void Handle(ImportMeterCommand message)
        {
            var connection = Configuration.CreateConnection();
            var domainRepository = new EventStoreDomainRepository(connection);

            Meter meter = Meter.Create(message.SerialNumber);

            meter.ChangeMeterState(Meter.MeterState.Imported);

            domainRepository.Save<Meter>(meter);

            Bus.Send(new ConfigureMeterCommand {SerialNumber=meter.SerialNumber });

        }
    }
}
