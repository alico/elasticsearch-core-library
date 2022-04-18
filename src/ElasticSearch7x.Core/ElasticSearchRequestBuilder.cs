using ElasticSearch7x.Core.Aggregations;
using ElasticSearch7x.Core.Extensions;
using ElasticSearch7x.Core.Helpers;
using ElasticSearch7x.Core.Interfaces;
using ElasticSearch7x.Core.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearch7x.Core
{
    public class ElasticSearchRequestBuilder<T> : IElasticSearchRequestBuilder<T> where T : class, new()
    {
        private int _from;
        private int _size;
        private string _indexName;
        private List<IElasticSearchQuery> _mustQueries;
        private List<IElasticSearchQuery> _mustNotQueries;
        private List<IElasticSearchQuery> _shouldQueries;
        private List<IElasticSearchFilter> _mustFilters;
        private List<IElasticSearchFilter> _mustNotFilters;
        private List<IElasticSearchFilter> _shouldFilters;
        protected List<IElasticSearchAggregation> _aggregations;
        private List<ISort> _sorts;
        private bool _trackScore;
        private ISourceFilter _sourceFilter;
        private ElasticSearchRequestBuilder()
        {
            _mustQueries = new List<IElasticSearchQuery>();
            _mustNotQueries = new List<IElasticSearchQuery>();
            _shouldQueries = new List<IElasticSearchQuery>();
            _mustFilters = new List<IElasticSearchFilter>();
            _mustNotFilters = new List<IElasticSearchFilter>();
            _shouldFilters = new List<IElasticSearchFilter>();
            _aggregations = new List<IElasticSearchAggregation>();
            _sorts = new List<ISort>();
        }

        public static IElasticSearchRequestBuilder<T> Init()
        {
            return new ElasticSearchRequestBuilder<T>();
        }

        public IElasticSearchRequestBuilder<T> From(int from)
        {
            _from = from;
            return this;
        }

        public IElasticSearchRequestBuilder<T> Size(int size)
        {
            _size = size;
            return this;
        }

        public IElasticSearchRequestBuilder<T> Source(ISourceFilter sourceFilter)
        {
            if (sourceFilter is not null)
                _sourceFilter = sourceFilter;

            return this;
        }

        public IElasticSearchRequestBuilder<T> Index(string indexName)
        {
            _indexName = indexName;
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToMustQueries(IElasticSearchQuery elasticSearchQuery)
        {
            _mustQueries.Add(elasticSearchQuery);
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToMustNotQueries(IElasticSearchQuery elasticSearchQuery)
        {
            _mustNotQueries.Add(elasticSearchQuery);
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToShouldQueries(IElasticSearchQuery elasticSearchQuery)
        {
            _shouldQueries.Add(elasticSearchQuery);
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToMustFilters(IElasticSearchFilter elasticSearchFilter)
        {
            _mustFilters.Add(elasticSearchFilter);
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToMustNotFilters(IElasticSearchFilter elasticSearchFilter)
        {
            _mustNotFilters.Add(elasticSearchFilter);
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToShouldFilters(IElasticSearchFilter elasticSearchFilter)
        {
            _shouldFilters.Add(elasticSearchFilter);
            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToGeoDistanceSort(GeoDistanceSortRequest request)
        {
            if (request != null)
            {
                _sorts.Add(new GeoDistanceSort
                {
                    Field = request.FieldName,
                    Points = new List<GeoLocation> { new GeoLocation(request.Latitude, request.Longitude) },
                    Unit = request.Unit.GetGeoUnit(),
                    Mode = request.SortType.GetSortMode(),
                    Order = request.Direction.GetSortOrder()
                });
            }

            return this;
        }

        public IElasticSearchRequestBuilder<T> AddToAggregations(IElasticSearchAggregation aggregation)
        {
            _aggregations.Add(aggregation);
            return this;
        }

        public IElasticSearchRequestBuilder<T> EnableTrackScore()
        {
            _trackScore = true;
            return this;
        }

        public IElasticSearchRequestBuilder<T> SetScoreSort()
        {
            AddSort("_score", SortOrder.Descending);
            EnableTrackScore();

            return this;
        }

        public IElasticSearchRequestBuilder<T> AddSort(string field, SortOrder order = SortOrder.Descending)
        {
            _sorts.Add(new FieldSort
            {
                Field = field,
                Order = order
            });

            return this;
        }

        public SearchRequest<T> Build()
        {
            return new SearchRequest<T>(_indexName)
            {
                From = _from,
                Size = _size,
                TrackScores = _trackScore,
                Sort = _sorts,
                Source = CreateSource(),
                Query = GenerateQuery(),
                Aggregations = CreateAggregations(),
            };
        }

        #region Private Methods
        private Union<bool, ISourceFilter> CreateSource()
        {
            if (_sourceFilter is not null)
                return new Union<bool, ISourceFilter>(_sourceFilter);

            return null;
        }

        private QueryContainer GenerateQuery()
        {
            var boolFilter = new BoolQuery();
            AssignFiltersToBoolQuery(boolFilter);

            var boolQuery = new BoolQuery();
            AssignQueriesToBoolQuery(boolQuery);

            var mainQuery = new BoolQuery();

            if (boolFilter.Must != null || boolFilter.MustNot != null || boolFilter.Should != null)
                mainQuery.Filter = new List<QueryContainer> { boolFilter };
            else
                mainQuery.Filter = new List<QueryContainer> { new MatchAllQuery() };


            if (boolQuery.Must != null || boolQuery.MustNot != null || boolQuery.Should != null)
                mainQuery.Must = new List<QueryContainer> { boolQuery };
            else
                mainQuery.Must = new List<QueryContainer> { new MatchAllQuery() };

            return mainQuery;
        }

        private void AssignQueriesToBoolQuery(BoolQuery boolQuery)
        {
            boolQuery.Must = GetBoolQueryGroup(_mustQueries);
            boolQuery.MustNot = GetBoolQueryGroup(_mustNotQueries);
            boolQuery.Should = GetBoolQueryGroup(_shouldQueries);
        }

        private void AssignFiltersToBoolQuery(BoolQuery boolQuery)
        {
            boolQuery.Must = GetBoolFilterGroup(_mustFilters);
            boolQuery.MustNot = GetBoolFilterGroup(_mustNotFilters);
            boolQuery.Should = GetBoolFilterGroup(_shouldFilters);
        }

        private IEnumerable<QueryContainer> GetBoolQueryGroup(IEnumerable<IElasticSearchQuery> queries)
        {
            if (queries != null && queries.Any())
            {
                var queryContainers = new List<QueryContainer>();

                foreach (var query in queries)
                {
                    query.AddToCollection(queryContainers);
                }

                return queryContainers;
            }

            return null;
        }

        private List<QueryContainer> GetBoolFilterGroup(IEnumerable<IElasticSearchFilter> filters, bool includeAggregationFilters = false, IEnumerable<Type> filterTypesToExclute = null)
        {
            if (filters != null && filters.Any())
            {
                var boolGroup = new List<QueryContainer>();

                foreach (var filter in filters)
                {
                    if ((filterTypesToExclute == null || (!filterTypesToExclute.Contains(filter.GetType()) && !filter.GetType().GetInterfaces().Any(x => filterTypesToExclute.Contains(x))))
                        && filter.IsAggregated() == includeAggregationFilters)
                    {
                        filter.AddToCollection(boolGroup);
                    }
                }
                if (boolGroup.Count > 0)
                {
                    return boolGroup;
                }
            }
            return null;

        }

        protected AggregationDictionary CreateAggregations(ISourceFilter sourceFilter = null)
        {
            var aggregationDictionary = new AggregationDictionary();
            foreach (var agg in _aggregations)
            {
                agg.AddToCollection(aggregationDictionary);
            }

            return aggregationDictionary;
        }

        protected BoolQuery AssignFiltersToBoolFilter(BoolQuery boolFilter, bool includeAggregationFilters, IEnumerable<Type> filterTypesToExclute = null)
        {
            boolFilter.Must = NestHelper.GetBoolFilterGroup(_mustFilters, includeAggregationFilters, filterTypesToExclute);
            boolFilter.MustNot = NestHelper.GetBoolFilterGroup(_mustNotFilters, includeAggregationFilters, filterTypesToExclute);
            boolFilter.Should = NestHelper.GetBoolFilterGroup(_shouldFilters, includeAggregationFilters, filterTypesToExclute);

            if (boolFilter.Must != null || boolFilter.MustNot != null || boolFilter.Should != null)
                return boolFilter;
            else
                return null;
        }

        #endregion
    }
}