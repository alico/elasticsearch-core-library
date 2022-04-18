using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Interfaces
{
    public interface IElasticSearchFilter
    {
        void AddToCollection(List<QueryContainer> filterCollection);
        bool IsAggregated();
    }
}
