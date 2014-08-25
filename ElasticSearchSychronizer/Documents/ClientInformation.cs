using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchSychronizer.Documents
{
    public class ClientInformation
    {
        public string ID { get; set; }

        public string Name { get; set; }

        [ElasticProperty(Type = FieldType.Double)]
        public double Balance { get; set; }

        [ElasticProperty(Type = FieldType.Date)]
        public DateTime LastMovement { get; set; }
    }
}
