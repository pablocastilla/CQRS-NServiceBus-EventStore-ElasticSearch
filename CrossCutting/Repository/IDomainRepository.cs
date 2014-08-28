using System;
using System.Collections.Generic;
using CrossCutting.DomainBase;

namespace CrossCutting.Repository
{
    public interface IDomainRepository
    {
        IEnumerable<IDomainEvent> Save<TAggregate>(TAggregate aggregate, bool isInitial = false) where TAggregate : IAggregate;
        TResult GetById<TResult>(string id) where TResult : IAggregate, new();
    }
}