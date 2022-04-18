using ElasticSearch7x.Core.Enums;

namespace ElasticSearch7x.Core.Models
{
    public class GeoDistanceSortRequest
    {
        public string FieldName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public GeoDistanceUnit Unit { get; set; }
        public SortType SortType { get; set; }
        public SortDirection Direction { get; set; }
    }
}