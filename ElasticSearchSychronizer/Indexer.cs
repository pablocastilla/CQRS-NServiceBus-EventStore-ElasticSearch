using System;
using Domain.Events;
using Nest;

namespace ElasticSearchSychronizer
{
    internal class Indexer
    {
        private readonly ElasticClient _esClient;
        private string _index = "MeterManager";

        public Indexer()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
            settings.SetDefaultIndex(_index);
            _esClient = new ElasticClient(settings);
        }

        public TDocument Get<TDocument>(Guid id) where TDocument : class
        {
            return _esClient.Get<TDocument>(id.ToString()).Source;
        }

        public void Index<TDocument>(TDocument document) where TDocument : class
        {
            _esClient.Index<TDocument>(document);
        }

        public void Init()
        {
            _esClient.CreateIndex(_index, y => y             
                .AddMapping<MeterCreated>(m => m.MapFromAttributes()));
        }
    }
}