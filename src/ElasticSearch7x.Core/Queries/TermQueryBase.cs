using ElasticSearch7x.Core.Extensions;
using Nest;

namespace ElasticSearch7x.Core.Queries
{
    public abstract class TermQueryBase : ElasticSearchQueryBase
    {
        private readonly bool _ignoreConvertToLower;
        private readonly string _fieldName;
        private readonly string _value;
        private readonly string _queryName;

        public TermQueryBase(string fieldName, string value, bool ignoreConvertToLower = false,
            string queryName = null)
        {
            _ignoreConvertToLower = ignoreConvertToLower;
            _queryName = queryName;
            _fieldName = fieldName;
            _value = value;
        }

        protected override QueryContainer CreateQuery()
        {
            QueryContainer queryContainer = null;

            if (!string.IsNullOrEmpty(_value))
            {
                var termQuery = new TermQuery
                {
                    Field = _fieldName,
                    Value = _ignoreConvertToLower ? _value : _value.ToLower()
                };

                if (!string.IsNullOrEmpty(_queryName))
                {
                    termQuery.Name = _queryName;
                }

                queryContainer = termQuery;
            }

            return queryContainer;
        }
    }
}