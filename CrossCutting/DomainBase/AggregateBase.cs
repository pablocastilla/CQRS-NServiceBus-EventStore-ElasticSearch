using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.DomainBase
{
    public abstract class AggregateBase : IAggregate
    {
        public abstract string AggregateId { get; }


        public int Version
        {
            get
            {
                return version;
            }
            protected set
            {
                version = value;
            }
        }

       
        private List<IDomainEvent> _uncommitedEvents = new List<IDomainEvent>();
        private Dictionary<Type, Action<IDomainEvent>> _routes = new Dictionary<Type, Action<IDomainEvent>>();
        private int version = -1;

        protected void RegisterTransition<T>(Action<T> transition) where T : class
        {
            _routes.Add(typeof(T), o => transition(o as T));
        }


        public void RaiseEvent(IDomainEvent @event)
        {
            ApplyEvent(@event);
            _uncommitedEvents.Add(@event);
        }

        public void ApplyEvent(IDomainEvent @event)
        {
            var eventType = @event.GetType();
            if (_routes.ContainsKey(eventType))
            {
                _routes[eventType](@event);
            }
            Version++;
        }

        public IEnumerable<IDomainEvent> UncommitedEvents()
        {
            return _uncommitedEvents;
        }

        public void ClearUncommitedEvents()
        {
            _uncommitedEvents.Clear();
        }    
     
    }
}
