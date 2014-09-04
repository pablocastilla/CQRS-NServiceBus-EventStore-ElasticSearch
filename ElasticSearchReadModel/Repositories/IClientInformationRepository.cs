using System;


namespace ElasticSearchReadModel.Repositories
{
    public interface IClientInformationRepository
    {
        System.Collections.Generic.List<ElasticSearchReadModel.Documents.ClientInformation> GetClientsBy(string name, bool? onlyPossiblyStolen);
    }
}
