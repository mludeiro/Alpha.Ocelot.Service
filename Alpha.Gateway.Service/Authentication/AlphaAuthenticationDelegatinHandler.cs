using System.Net;
using Consul;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Alpha.Gateway.Authentication;

public class AlphaAuthenticationDelegatinHandler( //IConsulClient consulClient, IMemoryCache memCache, 
    ILogger<AlphaAuthenticationDelegatinHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authHeader = request.Headers.Authorization;
        logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>>>>>> "+authHeader);

        if( authHeader is not null )
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;

            var response = await client.GetAsync("http://token:8080/api/check", cancellationToken);

            if( !response.IsSuccessStatusCode )
            {
                logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>>>>>> ERROR "+ response.StatusCode);

                // return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                // {
                //     Content = new StringContent("Alpha Gateway Authentication"),
                //     RequestMessage = request
                // };
            } else
                logger.LogInformation(">>>>>>>>>>>>>>>>>>>>>>>>>> SUCESS");
        }

        return await base.SendAsync(request, cancellationToken);
    }

    // private async Task<CatalogService?> GetRandomCatalogService(string serviceName, CancellationToken cancellationToken)
    // {
    //     var memKey = $"ConsulRegistryHandler/{serviceName}";
    //     var catalog = memCache.Get<QueryResult<CatalogService[]>>(memKey);

    //     if( catalog is null )
    //     {
    //         logger.LogInformation($"Colsul catalog request for service {serviceName}");
    //         catalog = await consulClient.Catalog.Service(serviceName,cancellationToken);
    //         memCache.Set(memKey, catalog, DateTimeOffset.Now.AddSeconds(30));
    //     }

    //     if(catalog.Response.Length == 0) return null;

    //     var randomPos = random.Next() % catalog.Response.Length;

    //     return catalog.Response[randomPos];
    // }
}