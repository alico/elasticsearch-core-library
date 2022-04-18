using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Models
{
    public class ElasticHit<T>
    {
        public T Document { get; set; }
        public IReadOnlyCollection<object> Sorts { get; set; }
        public double? Score { get; set; }
        public FieldValues Fields { get; set; }
        public AggregateDictionary Aggregations { get; set; }
    }
}