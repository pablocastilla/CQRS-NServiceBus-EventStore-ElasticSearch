using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages.Commands
{
    public class WithdrawMoneyCommand : BusMessage, ICommand
    {
        public string ClientID { get; set; }

        public double Quantity { get; set; }

        public bool FromATM { get; set; }
    }
}
