using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using ElasticSearchReadModel.Repositories;

namespace UI.App_Start
{
    public class ContainerConfig
    {
        public static void Initialize()
        {
            ObjectFactory.Initialize(o => o.For<IClientInformationRepository>().Use<ClientInformationRepository>());
        }
    }
}