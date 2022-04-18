using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Interfaces
{
    public interface IElasticSearchQuery
    {
        void AddToCollection(List<QueryContainer> filterCollection);
    }
}