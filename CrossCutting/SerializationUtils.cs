using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace CrossCutting
{
    public class SerializationUtils
    {

        public static string EventClrTypeHeader = "EventClrTypeName";

        public static T DeserializeObject<T>(byte[] data)
        {
            return (T)(DeserializeObject(data, typeof(T).AssemblyQualifiedName));
        }

        public static object DeserializeObject(byte[] data, string typeName)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject(jsonString, Type.GetType(typeName));
            }
            catch 
            {

                return null;
            }
        }

        public static object DeserializeEvent(RecordedEvent originalEvent)
        {
            if (originalEvent.Metadata != null)
            {
                var metadata = DeserializeObject<Dictionary<string, string>>(originalEvent.Metadata);
                if (metadata != null && metadata.ContainsKey(EventClrTypeHeader))
                {
                    var eventData = DeserializeObject(originalEvent.Data, metadata[EventClrTypeHeader]);
                    return eventData;
                }
            }
            return null;
        }
    }
}
