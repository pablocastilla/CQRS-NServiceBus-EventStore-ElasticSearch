using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchSychronizer.Documents
{
    public class DeviceInfo
    {
        public Guid Id { get; set; }

        public string SerialNumber { get; set; }

        public string State { get; set; }

        [ElasticProperty(Type = FieldType.Date)]
        public DateTime LastReadTime { get; set; }

        [ElasticProperty(Type = FieldType.Double)]
        public double LastReadValue { get; set; }

        [ElasticProperty(Type = FieldType.Date)]
        public DateTime LastUpdated { get; set; }
    }
}
