using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.DomainBase;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using StructureMap;

namespace CrossCutting.Repository
{
    public class EventStoreDomainRepository : DomainRepositoryBase
    {
        private IEventStoreConnection connection;
        private const string Category = "Bank";

        public EventStoreDomainRepository()
        {          
            ObjectFactory.Configure(c => c.For<IEventStoreConnection>().Use(Configuration.CreateConnection()));

            this.connection = ObjectFactory.GetInstance<IEventStoreConnection>(); ;
        }

        private string AggregateToStreamName(Type type, string id)
        {
            return string.Format("{0}-{1}-{2}", Category, type.Name, id);
        }

        public override IEnumerable<IDomainEvent> Save<TAggregate>(TAggregate aggregate, bool isInitial=false)
        {
            var events = aggregate.UncommitedEvents().ToList();
            //var expectedVersion = CalculateExpectedVersion(aggregate, events);

            var originalVersion = aggregate.Version - events.Count;

            var expectedVersion = originalVersion == -1 ? ExpectedVersion.NoStream : originalVersion;
            
            if(isInitial)              
                expectedVersion = ExpectedVersion.NoStream;

            var eventData = events.Select(CreateEventData);
            var streamName = AggregateToStreamName(aggregate.GetType(), aggregate.AggregateId);
            connection.AppendToStreamAsync(streamName, expectedVersion, eventData);
            return events;
        }

        public override TResult GetById<TResult>(string id) 
        {
            var streamName = AggregateToStreamName(typeof(TResult), id);
            var eventsSlice = connection.ReadStreamEventsForwardAsync(streamName, 0, int.MaxValue, false);
            if (eventsSlice.Result.Status == SliceReadStatus.StreamNotFound)
            {
                throw new AggregateNotFoundException("Could not found aggregate of type " + typeof(TResult) + " and id " + id);
            }
            var deserializedEvents = eventsSlice.Result.Events.Select(e =>
            {
                var metadata = SerializationUtils.DeserializeObject<Dictionary<string, string>>(e.OriginalEvent.Metadata);
                var eventData = SerializationUtils.DeserializeObject(e.OriginalEvent.Data, metadata[EventClrTypeHeader]);
                return eventData as IDomainEvent;
            });
            return BuildAggregate<TResult>(deserializedEvents);
        }

       

        public EventData CreateEventData(object @event)
        {
            var eventHeaders = new Dictionary<string, string>()
            {
                {
                    EventClrTypeHeader, @event.GetType().AssemblyQualifiedName
                },
                {
                    "Domain", "Devices"
                }
            };
            var eventDataHeaders = SerializeObject(eventHeaders);
            var data = SerializeObject(@event);
            var eventData = new EventData(Guid.NewGuid(), @event.GetType().Name, true, data, eventDataHeaders);
            return eventData;
        }

        private byte[] SerializeObject(object obj)
        {
            var jsonObj = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(jsonObj);
            return data;
        }

        public string EventClrTypeHeader = "EventClrTypeName";
    }
}