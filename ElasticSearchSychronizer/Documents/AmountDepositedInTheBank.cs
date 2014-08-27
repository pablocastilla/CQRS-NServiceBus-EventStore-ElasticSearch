using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchSychronizer.Documents
{
    public class AmountDepositedInTheBank
    {
        [ElasticProperty(Type = FieldType.Double)]
        public double Quantity { get; set; }

        [ElasticProperty(Type = FieldType.Date)]
        public DateTime TimeStamp { get; set; }
    }
}
