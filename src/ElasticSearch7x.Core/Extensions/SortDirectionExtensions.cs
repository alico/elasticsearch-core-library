using ElasticSearch7x.Core.Enums;
using Nest;
using System;

namespace ElasticSearch7x.Core.Extensions
{
    public static class SortDirectionExtensions
    {
        public static SortOrder GetSortOrder(this SortDirection value)
        {
            switch (value)
            {
                case SortDirection.Ascending:
                    return SortOrder.Ascending;
                case SortDirection.Descending:
                    return SortOrder.Descending;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
