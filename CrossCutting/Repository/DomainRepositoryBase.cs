using System;
using System.Collections.Generic;
using CrossCutting.DomainBase;

namespace CrossCutting.Repository
{
    public abstract class DomainRepositoryBase : IDomainRepository
    {
        public abstract IEnumerable<IDomainEvent> Save<TAggregate>(TAggregate aggregate, bool isInitial = false) where TAggregate : IAggregate;

        public abstract TResult GetById<TResult>(string id) where TResult : IAggregate, new();

        protected int CalculateExpectedVersion<T>(IAggregate aggregate, List<T> events)
        {
            var expectedVersion = aggregate.Version - events.Count;
            return expectedVersion;
        }

        protected TResult BuildAggregate<TResult>(IEnumerable<IDomainEvent> events) where TResult : IAggregate, new()
        {
            var result = new TResult();
            foreach (var @event in events)
            {
                result.ApplyEvent(@event);
            }
            return result;
        }
    }
}