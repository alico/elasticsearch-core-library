using ElasticSearch7x.Core.Aggregations;
using ElasticSearch7x.Core.Models;
using Nest;

namespace ElasticSearch7x.Core.Interfaces
{
    public interface IElasticSearchRequestBuilder<T> where T : class, new()
    {
        IElasticSearchRequestBuilder<T> From(int from);
        IElasticSearchRequestBuilder<T> Size(int size);
        IElasticSearchRequestBuilder<T> Source(ISourceFilter sourceFilter);
        IElasticSearchRequestBuilder<T> Index(string indexName);
        IElasticSearchRequestBuilder<T> AddToMustQueries(IElasticSearchQuery elasticSearchQuery);
        IElasticSearchRequestBuilder<T> AddToMustNotQueries(IElasticSearchQuery elasticSearchQuery);
        IElasticSearchRequestBuilder<T> AddToShouldQueries(IElasticSearchQuery elasticSearchQuery);
        IElasticSearchRequestBuilder<T> AddToMustFilters(IElasticSearchFilter elasticSearchFilter);
        IElasticSearchRequestBuilder<T> AddToMustNotFilters(IElasticSearchFilter elasticSearchFilter);
        IElasticSearchRequestBuilder<T> AddToShouldFilters(IElasticSearchFilter elasticSearchFilter);
        IElasticSearchRequestBuilder<T> AddToGeoDistanceSort(GeoDistanceSortRequest request);
        IElasticSearchRequestBuilder<T> EnableTrackScore();
        IElasticSearchRequestBuilder<T> SetScoreSort();
        IElasticSearchRequestBuilder<T> AddSort(string field, SortOrder order = SortOrder.Descending);
        IElasticSearchRequestBuilder<T> AddToAggregations(IElasticSearchAggregation aggregation);

        SearchRequest<T> Build();
    }
}