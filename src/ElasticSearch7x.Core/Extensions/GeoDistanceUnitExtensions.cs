using ElasticSearch7x.Core.Enums;
using Nest;
using System;

namespace ElasticSearch7x.Core.Extensions
{
    public static class GeoDistanceUnitExtensions
    {
        public static DistanceUnit GetGeoUnit(this GeoDistanceUnit value)
        {
            switch (value)
            {
                case GeoDistanceUnit.Inch:
                    return DistanceUnit.Inch;
                case GeoDistanceUnit.Feet:
                    return DistanceUnit.Feet;
                case GeoDistanceUnit.Yards:
                    return DistanceUnit.Yards;
                case GeoDistanceUnit.Miles:
                    return DistanceUnit.Miles;
                case GeoDistanceUnit.NauticalMiles:
                    return DistanceUnit.NauticalMiles;
                case GeoDistanceUnit.Kilometers:
                    return DistanceUnit.Kilometers;
                case GeoDistanceUnit.Meters:
                    return DistanceUnit.Meters;
                case GeoDistanceUnit.Centimeters:
                    return DistanceUnit.Centimeters;
                case GeoDistanceUnit.Millimeters:
                    return DistanceUnit.Millimeters;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
