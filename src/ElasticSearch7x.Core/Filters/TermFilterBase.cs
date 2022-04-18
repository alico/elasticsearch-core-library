using Nest;
using System.Collections.Generic;

namespace ElasticSearch7x.Core.Filters
{
    public abstract class TermFilterBase<T> : ElasticSearchFilterBase
    {
        private readonly string _fieldName;
        private readonly T[] _values;
        private readonly string _filterName;

        public TermFilterBase(string fieldName, T[] value, bool isAggregated = false, string filterName = null)
            : base(isAggregated)
        {
            _fieldName = fieldName;
            _values = value;
            _filterName = filterName;
        }

        protected override QueryContainer CreateFilterQuery()
        {
            QueryContainer queryContainer = null;

            if (_values != null && _values.Length == 1)
            {
                var termFilter = new TermQuery()
                {
                    Field = _fieldName,
                    Value = _values[0]
                };

                if (!string.IsNullOrEmpty(_fieldName))
                {
                    termFilter.Name = _fieldName;
                }

                queryContainer = termFilter;
            }
            else if (_values != null && _values.Length > 1)
            {
                List<QueryContainer> should = new List<QueryContainer>();

                foreach (var value in _values)
                {
                    var termFilter = new TermQuery()
                    {
                        Field = _fieldName,
                        Value = value
                    };

                    should.Add(termFilter);
                }
                queryContainer = new BoolQuery()
                {
                    Should = should,
                    Name = _filterName
                };
            }
            return queryContainer;
        }
    }
}
