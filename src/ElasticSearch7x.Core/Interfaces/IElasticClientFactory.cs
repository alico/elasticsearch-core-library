using Nest;

namespace ElasticSearch7x.Core.Interfaces
{
    public interface IElasticClientFactory
    {
        IElasticClientFactory ServerUrl(string url);
        IElasticClientFactory UserName(string userName);
        IElasticClientFactory Password(string password);
        IElasticClientFactory DefaultIndex(string indexName);
        IElasticClient Create();
    }
}