using ElasticSearch7x.Core.Enums;
using Nest;
using System;

namespace ElasticSearch7x.Core.Extensions
{
    public static class SortTypeExtensions
    {
        public static SortMode GetSortMode(this SortType value)
        {
            switch (value)
            {
                case SortType.Min:
                    return SortMode.Min;
                case SortType.Max:
                    return SortMode.Max;
                case SortType.Sum:
                    return SortMode.Sum;
                case SortType.Average:
                    return SortMode.Average;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
