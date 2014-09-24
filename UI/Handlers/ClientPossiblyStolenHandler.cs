using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Messages.Events;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace UI.Handlers
{
    public class ClientPossiblyStolenHandler : IHandleMessages<ClientPossiblyStolen>
    {
        public void Handle(ClientPossiblyStolen message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<NonPersistentHub>();
            hub.Clients.All.clientPossiblyStolen(message);
        }
    }
}