using ElasticSearch7x.Core.Interfaces;
using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Queries
{
    public abstract class ElasticSearchQueryBase : IElasticSearchQuery
    {
        public ElasticSearchQueryBase()
        {
        }

        protected abstract QueryContainer CreateQuery();

        public virtual void AddToCollection(List<QueryContainer> queryContainers)
        {
            QueryContainer filter = CreateQuery();

            if (filter != null)
            {
                queryContainers.Add(filter);
            }
        }
    }
}