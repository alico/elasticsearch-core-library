using ElasticSearch7x.Core.Extensions;
using Nest;
using System.Linq;

namespace ElasticSearch7x.Core.Queries
{
    public abstract class TermsQueryBase : ElasticSearchQueryBase
    {
        private readonly bool _ignoreEmptyValues;
        private readonly bool _ignoreConvertToLower;
        private readonly string _fieldName;
        private readonly string[] _values;
        private readonly string _queryName;

        public TermsQueryBase(string fieldName, string[] values, bool ignoreEmptyValues = true, bool ignoreConvertToLower = false,
            string queryName = null)
        {
            _ignoreEmptyValues = ignoreEmptyValues;
            _ignoreConvertToLower = ignoreConvertToLower;
            _queryName = queryName;
            _fieldName = fieldName;
            _values = values;
        }

        protected override QueryContainer CreateQuery()
        {
            QueryContainer queryContainer = null;

            if (_values.Any())
            {
                var terms = _ignoreEmptyValues ?
                    _values.Where(a => !string.IsNullOrWhiteSpace(a)) : _values;

                var termsQuery = new TermsQuery
                {
                    Field = _fieldName,
                    Terms = terms.Select(a => _ignoreConvertToLower ? a : a.ToLower())
                };

                if (!string.IsNullOrEmpty(_queryName))
                {
                    termsQuery.Name = _queryName;
                }

                queryContainer = termsQuery;
            }

            return queryContainer;
        }
    }
}