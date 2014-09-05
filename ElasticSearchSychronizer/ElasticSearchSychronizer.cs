using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CrossCutting;
using CrossCutting.Repository;
using Domain.Events;
using ElasticSearchReadModel.Documents;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Topshelf;

namespace ElasticSearchSychronizer
{
    public class ElasticSearchSychronizer
    {
        private Indexer indexer;
        private Dictionary<Type, Action<object>> eventHandlerMapping;
        private Position? latestPosition;
        private IEventStoreConnection connection;

        
        public ElasticSearchSychronizer()
        {
            
        }
        public void Start() 
        {
            indexer = new Indexer();
            indexer.Init();
            connection = Configuration.CreateConnection();

            eventHandlerMapping = CreateEventHandlerMapping();
            ConnectToEventstore();
        
        }
        public void Stop() 
        { 
           
        }


        private void ConnectToEventstore()
        {

            latestPosition = Position.Start;
           
            var subs = connection.SubscribeToAllFrom(latestPosition,true, HandleEvent);

           
            //subscribe to the stream of fraud alert
            connection.SubscribeToStreamFrom("PossiblyStolenCardClients", 0, true,

               (sub, e) =>
               {

                   var jsonString = Encoding.UTF8.GetString(e.Event.Data);

                   JObject o = JObject.Parse(jsonString);


                   var clientID = (string)o["ClientID"];

                   var ci = indexer.Get<ClientInformation>(clientID);

                   if (ci == null)
                       ci = new ClientInformation() { ID = clientID };

                   ci.PossiblyStolen = true;

                   indexer.Index(ci);
               }
               );
            
           
            Console.WriteLine("Indexing service started");
        }

        private void HandleEvent(EventStoreCatchUpSubscription arg1, ResolvedEvent arg2)
        {
            var @event = SerializationUtils.DeserializeEvent(arg2.OriginalEvent);
            if (@event != null)
            {
                var eventType = @event.GetType();
                if (eventHandlerMapping.ContainsKey(eventType))
                {
                    eventHandlerMapping[eventType](@event);
                }
            }
            latestPosition = arg2.OriginalPosition;
        }

        private Dictionary<Type, Action<object>> CreateEventHandlerMapping()
        {
            return new Dictionary<Type, Action<object>>()
            {
                {typeof (ClientCreated), o => Handle(o as ClientCreated)},
                {typeof (MoneyDeposited), o => Handle(o as MoneyDeposited)},
                {typeof (MoneyWithdrawn), o => Handle(o as MoneyWithdrawn)}              
             
            };
        }

        private void Handle(ClientCreated evt)
        {
            var ci = indexer.Get<ClientInformation>(evt.ID);

            ClientInformation clientInfo;

            if(ci==null)
                clientInfo = new ClientInformation() 
                            {                                
                               ID=evt.ID,
                               Name=evt.Name,
                               Balance=0
                            };
            else
            {
                clientInfo = ci;
                clientInfo.Balance = 0;
                clientInfo.Name = evt.Name;          
            }



            indexer.Index(clientInfo);
        }

        private void Handle(MoneyDeposited evt)
        {
            var ci = indexer.Get<ClientInformation>(evt.ClientID);

            ci.Balance += evt.Quantity;
            ci.LastMovement = evt.TimeStamp;

            indexer.Index(ci);

            var ad = indexer.Get<AmountDepositedInTheBank>(evt.TransactionId.ToString());

            if(ad==null)
                indexer.Index(new AmountDepositedInTheBank { Quantity = evt.Quantity, TimeStamp = evt.TimeStamp, ID=evt.ClientID,TransactionId=evt.TransactionId });
        }

        private void Handle(MoneyWithdrawn evt)
        {
            var ci = indexer.Get<ClientInformation>(evt.ClientID);

            ci.Balance -= evt.Quantity;
            ci.LastMovement = evt.TimeStamp;

            indexer.Index(ci);

            var ad = indexer.Get<AmountDepositedInTheBank>(evt.TransactionId.ToString());

            if(ad==null)
                indexer.Index(new AmountDepositedInTheBank { Quantity = evt.Quantity, TimeStamp = evt.TimeStamp, ID=evt.ClientID,TransactionId=evt.TransactionId });
        }
      
    }

    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>                                 //1
            {
                x.Service<ElasticSearchSychronizer>(s =>                        //2
                {
                    s.ConstructUsing(name => new ElasticSearchSychronizer());     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Service that synchronizes EventStore with Elastic Search");        //7
                x.SetDisplayName("ElasticSearchSynchronizer");                       //8
                x.SetServiceName("ElasticSearchSynchronizer");                       //9
            });                                                  //10
        }
    }
}
