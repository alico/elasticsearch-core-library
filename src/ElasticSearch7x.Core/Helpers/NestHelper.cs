using ElasticSearch7x.Core.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearch7x.Core.Helpers
{
    public static class NestHelper
    {
        public static List<QueryContainer> GetBoolFilterGroup(IEnumerable<IElasticSearchFilter> filters, bool includeAggregationFilters = false, IEnumerable<Type> filterTypesToExclute = null)
        {
            if (filters != null && filters.Any())
            {
                List<QueryContainer> boolGroup = new List<QueryContainer>();

                foreach (IElasticSearchFilter filter in filters)
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

        public static IEnumerable<QueryContainer> GetBoolQueryGroup(IEnumerable<IElasticSearchQuery> queries)
        {
            if (queries != null && queries.Any())
            {
                var queryContainers = new List<QueryContainer>();

                foreach (IElasticSearchQuery query in queries)
                {
                    query.AddToCollection(queryContainers);
                }
                return queryContainers;
            }
            else
                return null;

        }

        public static QueryContainer MergeIntoFilteredQuery(BoolQuery boolFilter, BoolQuery boolQuery)
        {
            BoolQuery mainBoolQuery = new BoolQuery();

            if (boolQuery != null && (boolQuery.Must != null || boolQuery.MustNot != null || boolQuery.Should != null))
            {
                mainBoolQuery.Must = boolQuery.Must;
                mainBoolQuery.Should = boolQuery.Should;
                mainBoolQuery.MustNot = boolQuery.MustNot;
            }
            else
                mainBoolQuery.Must = new List<QueryContainer>() { new MultiMatchQuery() };

            if (boolFilter != null && (boolFilter.Must != null || boolFilter.MustNot != null || boolFilter.Should != null))
                mainBoolQuery.Filter = new List<QueryContainer>()
                {
                    new BoolQuery()
                    {
                        Must = boolFilter.Must,
                        Should = boolFilter.Should,
                        MustNot = boolFilter.MustNot
                    }
                };
            else
                mainBoolQuery.Filter = new List<QueryContainer>() { new MultiMatchQuery() };

            return mainBoolQuery;
        }
    }
}
