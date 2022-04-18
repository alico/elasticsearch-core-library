using System.ComponentModel;

namespace ElasticSearch7x.Core.Enums
{
    public enum GeoDistanceUnit
    {
        Inch = 0,
        Feet = 1,
        Yards = 2,
        [Description("mi")]
        Miles = 3,
        NauticalMiles = 4,
        [Description("km")]
        Kilometers = 5,
        [Description("m")]
        Meters = 6,
        [Description("cm")]
        Centimeters = 7,
        [Description("mm")]
        Millimeters = 8
    }
}