using ElasticSearch7x.Core.Extensions;
using ElasticSearch7x.Core.Interfaces;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSearch7x.Core
{
    public abstract class ElasticSearchBase<T> where T : class, new()
    {
        private readonly IElasticClient _elasticClient;
        protected string LastExecutedQueryJson;

        protected ElasticSearchBase(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        protected virtual async Task<ISearchResponse<T>> SearchInnerAsync(IElasticSearchRequestBuilder<T> requestBuilder, CancellationToken cancellationToken)
        {
            var request = requestBuilder.Build();

            LastExecutedQueryJson = _elasticClient.GetExecutedQueryJSonFromRequest(request);

            var result = await _elasticClient.SearchAsync<T>(request, cancellationToken);

            if (!result.IsValid)
            {
                throw new Exception("Elastic Query Error. Query > " + result.ServerError.Error.Reason);
            }

            return result;
        }
    }
}