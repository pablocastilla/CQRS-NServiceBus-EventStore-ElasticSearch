using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElasticSearchReadModel.Documents;
using ElasticSearchReadModel.Repositories;
using StructureMap;

namespace UI.Controllers
{
    public class ClientsController : ApiController
    {
        // GET: api/Client
        public IEnumerable<ClientInformation> Get()
        {
            var rep = ObjectFactory.GetInstance<IClientInformationRepository>(); 

            return rep.GetClientsBy(null,null);
        }

        // GET: api/Client/pepe
        public IEnumerable<ClientInformation> Get(string clientName,string possiblyStolen=null)
        {
            var rep = new ClientInformationRepository();

            bool? possiblyStolenArgument = string.IsNullOrEmpty(possiblyStolen) || !bool.Parse(possiblyStolen) ? (bool?)null : true ;

            return rep.GetClientsBy(clientName, possiblyStolenArgument);
        }

        // POST: api/Client
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Client/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
    }
}
