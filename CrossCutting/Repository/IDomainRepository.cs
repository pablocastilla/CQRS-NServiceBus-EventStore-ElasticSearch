using System;
using System.Collections.Generic;
using CrossCutting.DomainBase;

namespace CrossCutting.Repository
{
    public interface IDomainRepository
    {
        IEnumerable<IDomainEvent> Save<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;
        TResult GetById<TResult>(Guid id) where TResult : IAggregate, new();
    }
}