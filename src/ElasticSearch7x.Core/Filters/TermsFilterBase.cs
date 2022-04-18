using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Filters
{
    public abstract class TermsFilterBase<T> : ElasticSearchFilterBase
    {
        private readonly string _fieldName;
        private readonly T[] _value;
        private readonly string _filterName;

        public TermsFilterBase(string fieldName, T[] value, bool isAggregated = false, string filterName = null)
            : base(isAggregated)
        {
            _fieldName = fieldName;
            _value = value;
            _filterName = filterName;
        }

        protected override QueryContainer CreateFilterQuery()
        {
            var termsFilter = new TermsQuery
            {
                Field = _fieldName,
                Terms = GetObjectArray()
            };

            if (!string.IsNullOrEmpty(_filterName))
            {
                termsFilter.Name = _filterName;
            }

            return termsFilter;
        }

        private List<object> GetObjectArray()
        {
            var obj = new List<object>();

            foreach (var element in _value)
            {
                obj.Add(element);
            }

            return obj;
        }
    }
}
