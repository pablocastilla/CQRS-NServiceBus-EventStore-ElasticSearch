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

            Meter meter = Meter.Create(message.MeterId, message.SerialNumber);

            meter.ChangeMeterState(Meter.MeterState.Imported);

            domainRepository.Save<Meter>(meter);

          /*  Random rnd = new Random();
            int value = rnd.Next(1, 1000);

            Bus.Publish(new LoadProfileReceivedEvent()
            {
                MessageId = System.Guid.NewGuid(),
                MeterId = meter.Id,
                EntryDateTime = DateTime.Today,
                ReadTimeStamp = DateTime.Today,
                SerialNumber = message.SerialNumber,
                Value = value
            });*/
        }
    }
}
