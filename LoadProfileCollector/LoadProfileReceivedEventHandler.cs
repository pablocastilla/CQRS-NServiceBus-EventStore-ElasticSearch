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
        public void Handle(LoadProfileReceivedEvent message)
        {
            var connection = Configuration.CreateConnection();
            var domainRepository = new EventStoreDomainRepository(connection);
           
            var meter = domainRepository.GetById<Meter>(message.MeterId);

            meter.AddLoadProfile(
                new LoadProfileReceived()
                            {
                                Id = meter.Id,
                                LPReads = new List<LoadProfileRead>() 
                                { 
                                    new LoadProfileRead()
                                    {
                                        Id=meter.Id,
                                        EntryDateTime = message.EntryDateTime,
                                        ReadTimeStamp = message.ReadTimeStamp,
                                        Value = message.Value
                                    }
                                }
                            });

            meter.ChangeMeterState(Meter.MeterState.Operative);

            domainRepository.Save<Meter>(meter);
        }
    }
}
