using Nest;
using System.IO;

namespace ElasticSearch7x.Core.Extensions
{
    public static class ElasticClientExtensions
    {
        public static string GetExecutedQueryJSonFromRequest<T>(this IElasticClient elasticClient, SearchRequest<T> request)
        {
            using (var stream = new MemoryStream())
            {
                elasticClient.SourceSerializer.Serialize(request, stream);

                return System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}