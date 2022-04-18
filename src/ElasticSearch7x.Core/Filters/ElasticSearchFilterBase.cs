using ElasticSearch7x.Core.Interfaces;
using Nest;
using System;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Filters
{
    public abstract class ElasticSearchFilterBase : IElasticSearchFilter
    {
        private bool _IsAggregated;

        public ElasticSearchFilterBase(bool isAggregated = false)
        {
            _IsAggregated = isAggregated;
        }

        protected abstract QueryContainer CreateFilterQuery();

        public void AddToCollection(List<QueryContainer> queryContainers)
        {
            if (queryContainers == null)
            {
                throw new ArgumentNullException(nameof(queryContainers));
            }

            QueryContainer filter = CreateFilterQuery();

            if (filter != null)
            {
                queryContainers.Add(filter);
            }
        }

        public bool IsAggregated()
        {
            return _IsAggregated;
        }
    }
}
