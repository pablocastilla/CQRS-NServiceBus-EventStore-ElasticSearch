using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Repository;
using Domain;
using Domain.Events;
using Messages;
using NServiceBus;

namespace LoadProfileCollector
{
    public class LoadProfileReceivedEventHandler : IHandleMessages<LoadProfileReceivedEvent>
    {
        public IBus Bus { get; set; }

        public void Handle(LoadProfileReceivedEvent message)
        {
            var connection = Configuration.CreateConnection();
            var domainRepository = new EventStoreDomainRepository(connection);
           
            var meter = domainRepository.GetById<Meter>(message.SerialNumber);

            meter.AddLoadProfile(
                new LoadProfileReceived()
                            {                             
                                SerialNumber=message.SerialNumber,
                                LPReads = message.LoadProfile.Select(lp => new  LoadProfileRead()
                                            {
                                                ReadTimeStamp = lp.ReadTimeStamp,
                                                Value = lp.Value,
                                                EntryDateTime = lp.EntryDateTime
                                            }
                                            ).ToList()
                            });

            meter.ChangeMeterState(Meter.MeterState.Operative);

            domainRepository.Save<Meter>(meter);
        }
    }
}
