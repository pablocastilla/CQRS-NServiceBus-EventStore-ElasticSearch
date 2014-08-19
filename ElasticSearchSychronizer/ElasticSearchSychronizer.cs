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
                {typeof (MeterCreated), o => Handle(o as MeterCreated)},
                {typeof (LoadProfileReceived), o => Handle(o as LoadProfileReceived)},
                {typeof (MeterStateChanged), o => Handle(o as MeterStateChanged)}                
             
            };
        }

        private void Handle(MeterCreated evt)
        {
            var deviceInfo = new DeviceInfo() 
                            { 
                                Id = evt.Id,
                                SerialNumber = evt.SerialNumber
                            };

            deviceInfo.LastUpdated = DateTime.UtcNow;

            indexer.Index(deviceInfo);
        }

        private void Handle(LoadProfileReceived evt)
        {
            var di = indexer.Get<DeviceInfo>(evt.Id);


            di.LastReadTime = evt.LPReads.Max( lp => lp.ReadTimeStamp);
            di.LastReadValue = evt.LPReads.Max(lp => lp.Value);

            di.LastUpdated = DateTime.UtcNow;

            indexer.Index(di);
        }

        private void Handle(MeterStateChanged evt)
        {
            var di = indexer.Get<DeviceInfo>(evt.Id);

            di.State = evt.State.ToString();
            di.LastUpdated = DateTime.UtcNow;
            indexer.Index(di);
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

                x.SetDescription("Sample Topshelf Host");        //7
                x.SetDisplayName("Stuff");                       //8
                x.SetServiceName("stuff");                       //9
            });                                                  //10
        }
    }
}
