using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Models
{
    public class ElasticSearchResult<T> where T : class, new()
    {
        public IEnumerable<ElasticHit<T>> ElasticHit { get; set; }
        public Dictionary<string, List<KeyValuePair<string, long>>> Facets { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public long Total { get; set; }
        public string ExecutedQueryJSon { get; set; }
        public AggregateDictionary Aggregations { get; set; }

    }
}