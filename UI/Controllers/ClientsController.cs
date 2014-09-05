using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElasticSearchReadModel.Documents;
using ElasticSearchReadModel.Repositories;
using Messages.Commands;
using NServiceBus;
using StructureMap;
using UI.Controllers.DTOs;

namespace UI.Controllers
{
    public class ClientsController : ApiController
    {
        private static IBus bus;

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
        public void Post([FromBody] NewClientDTO newClient)
        {
            MvcApplication.Bus.Send("CreateClient", new CreateClientCommand() 
                                        {
                                            ClientID=newClient.id,
                                            Name=newClient.name,
                                            InitialDeposit=newClient.initialDeposit 
                                        });
        }

        // PUT: api/Client/5
        public void Put([FromBody] UpdateClientDTO updateClient)
        {
            if (updateClient.quantity >= 0)
            {
                MvcApplication.Bus.Send("DepositMoney", new DepositMoneyCommand()
                {
                    ClientID = updateClient.id,
                    Quantity = updateClient.quantity,
                    FromATM = bool.Parse(updateClient.inATM?? "False")                 
                });
            }
            else
            {
                MvcApplication.Bus.Send("WithdrawMoney", new WithdrawMoneyCommand()
                {
                    ClientID = updateClient.id,
                    Quantity = updateClient.quantity,
                    FromATM = bool.Parse(updateClient.inATM ?? "False")
                });
            }
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
    }
}
