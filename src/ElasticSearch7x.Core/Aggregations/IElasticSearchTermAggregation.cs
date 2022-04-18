using Nest;
using System;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Aggregations
{
    public interface IElasticSearchTermAggregation : IElasticSearchAggregation
    {
        void AssignFilter(BoolQuery query);
        IEnumerable<Type> FilterTypesToExclude();
    }
}