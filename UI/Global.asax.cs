using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using NServiceBus.Installation.Environments;

namespace UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IBus bus;

        private IStartableBus startableBus;

        public static IBus Bus
        {
            get { return bus; }
        }


        protected void Application_Start()
        {

            Configure.ScaleOut(s => s.UseSingleBrokerQueue());

            startableBus = Configure.With()
                .DefaultBuilder()
                .UseTransport<Msmq>()
                .PurgeOnStartup(false)
                .UnicastBus()
                .RunHandlersUnderIncomingPrincipal(false)
                .CreateBus();


            Configure.Instance.ForInstallationOn<Windows>().Install();

            bus = startableBus.Start();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            startableBus.Dispose();
        }
    }
}
