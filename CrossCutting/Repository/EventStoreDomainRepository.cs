using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.DomainBase;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace CrossCutting.Repository
{
    public class EventStoreDomainRepository : DomainRepositoryBase
    {
        private IEventStoreConnection _connection;
        private const string Category = "MeterManager";

        public EventStoreDomainRepository(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        private string AggregateToStreamName(Type type, Guid id)
        {
            return string.Format("{0}-{1}-{2}", Category, type.Name, id);
        }

        public override IEnumerable<IDomainEvent> Save<TAggregate>(TAggregate aggregate)
        {
            var events = aggregate.UncommitedEvents().ToList();
            //var expectedVersion = CalculateExpectedVersion(aggregate, events);

            var originalVersion = aggregate.Version - events.Count;
            var expectedVersion = originalVersion == -1 ? ExpectedVersion.NoStream : originalVersion;

            var eventData = events.Select(CreateEventData);
            var streamName = AggregateToStreamName(aggregate.GetType(), aggregate.Id);
            _connection.AppendToStream(streamName, expectedVersion, eventData);
            return events;
        }

        public override TResult GetById<TResult>(Guid id)
        {
            var streamName = AggregateToStreamName(typeof(TResult), id);
            var eventsSlice = _connection.ReadStreamEventsForward(streamName, 0, int.MaxValue, false);
            if (eventsSlice.Status == SliceReadStatus.StreamNotFound)
            {
                throw new AggregateNotFoundException("Could not found aggregate of type " + typeof(TResult) + " and id " + id);
            }
            var deserializedEvents = eventsSlice.Events.Select(e =>
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