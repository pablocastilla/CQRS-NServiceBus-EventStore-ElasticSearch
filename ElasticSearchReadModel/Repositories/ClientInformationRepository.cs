using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ElasticSearchReadModel.Documents;
using Nest;

namespace ElasticSearchReadModel.Repositories
{
    public class ClientInformationRepository : ElasticSearchReadModel.Repositories.IClientInformationRepository
    {
        private const string INDEX = "metermanager";

        public List<ClientInformation> GetClientsBy(string name, bool? onlyPossiblyStolen)
        {
            ElasticClient esClient;
                        
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"));
            settings.SetDefaultIndex(INDEX);

            esClient = new ElasticClient(settings);

            //text is always in lowercase in ES
            if (!string.IsNullOrEmpty(name))
                name = name.ToLowerInvariant();

            var result = esClient.Search<ClientInformation>(
               sd => sd.Query(  q=> q.Strict(false).Wildcard(t => t.Name,name))
                   .Filter(f => 
                       f.Strict(false).Term(t => t.PossiblyStolen, onlyPossiblyStolen))                  
                   
                   );

            return result.Documents.ToList();
        }

    }
}
