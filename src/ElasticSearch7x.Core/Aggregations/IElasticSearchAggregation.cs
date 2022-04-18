using Nest;

namespace ElasticSearch7x.Core.Aggregations
{
    public interface IElasticSearchAggregation
    {
        void AddToCollection(AggregationDictionary aggregationDictionary);
    }
}