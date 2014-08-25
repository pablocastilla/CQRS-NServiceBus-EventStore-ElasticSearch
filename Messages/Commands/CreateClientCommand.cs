using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages.Commands
{
    public class CreateClientCommand : BusMessage, ICommand
    {
        public string ID { get; set; }
        
        public string Name { get; set; }

        public double InitialDeposit { get; set; }

        
    }
}
