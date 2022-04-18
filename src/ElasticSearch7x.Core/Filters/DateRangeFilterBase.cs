using Nest;
using System;

namespace ElasticSearch7x.Core.Filters
{
    public abstract class DateRangeFilterBase : ElasticSearchFilterBase
    {
        readonly string _Field;
        readonly DateTime _Maximum;
        readonly DateTime _Minumum;
        readonly string _DateFormat;
        readonly string _FilterName;

        public DateRangeFilterBase(string field, DateTime minimum, DateTime maximum, string dateFormat, bool isAggregated = false, string filterName = null)
            : base(isAggregated)
        {
            _Field = field;
            _Maximum = maximum;
            _Minumum = minimum;
            _DateFormat = dateFormat;
            _FilterName = filterName;
        }

        protected override QueryContainer CreateFilterQuery()
        {
            var rangeFilter = new TermRangeQuery()
            {
                Field = _Field,
                LessThanOrEqualTo = _Maximum.ToString(_DateFormat),
                GreaterThanOrEqualTo = _Minumum.ToString(_DateFormat),
            };

            if (!string.IsNullOrEmpty(_FilterName))
            {
                rangeFilter.Name = _FilterName;
            }

            return rangeFilter;
        }
    }
}
