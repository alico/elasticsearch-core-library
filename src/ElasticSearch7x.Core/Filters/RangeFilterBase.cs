using Nest;
using System;
using ValueType = System.ValueType;

namespace ElasticSearch7x.Core.Filters
{
    public abstract class RangeFilterBase<T> : ElasticSearchFilterBase
    {
        readonly string _Field;
        readonly T _Maximum;
        readonly T _Minimum;
        readonly string _FilterName;

        public RangeFilterBase(string field, T minimum, T maximum, bool isAggregated = false, string filterName = null)
            : base(isAggregated)
        {
            _Field = field;
            _Maximum = maximum;
            _Minimum = minimum;
            _FilterName = filterName;
        }

        protected override QueryContainer CreateFilterQuery()
        {
            if (!(_Minimum is ValueType) && Equals(_Minimum, default(T)) && !(_Maximum is ValueType) && Equals(_Maximum, default(T)))
            {
                throw new Exception("Hem minimum, hem maximum null olamaz");
            }

            var rangeFilter = new TermRangeQuery()
            {
                Field = _Field,
                LessThanOrEqualTo = _Maximum is ValueType || !Equals(_Maximum, default(T)) ? _Maximum.ToString() : null,
                GreaterThanOrEqualTo = _Minimum is ValueType || !Equals(_Minimum, default(T)) ? _Minimum.ToString() : null
            };

            if (!Equals(_FilterName, default(T)))
            {
                rangeFilter.Name = _FilterName;
            }

            return rangeFilter;
        }
    }
}
