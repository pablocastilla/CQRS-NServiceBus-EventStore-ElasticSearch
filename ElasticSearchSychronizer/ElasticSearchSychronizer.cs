using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CrossCutting;
using CrossCutting.Repository;
using Domain.Events;
using ElasticSearchSychronizer.Documents;
using EventStore.ClientAPI;
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

            connection.SubscribeToAllFrom(latestPosition,true, HandleEvent);            
            
           
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
                {typeof (AmountDeposited), o => Handle(o as AmountDeposited)}              
             
            };
        }

        private void Handle(ClientCreated evt)
        {
            var clientInfo = new ClientInformation() 
                            {                                
                               
                            };

            /*deviceInfo.LastUpdated = DateTime.UtcNow;

            indexer.Index(deviceInfo);*/
        }

        private void Handle(AmountDeposited evt)
        {
            /*var di = indexer.Get<ClientInformation>(evt.SerialNumber);


            di.LastReadTime = evt.LPReads.Max( lp => lp.ReadTimeStamp);
            di.LastReadValue = evt.LPReads.Max(lp => lp.Value);

            di.LastUpdated = DateTime.UtcNow;

            indexer.Index(di);*/
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
