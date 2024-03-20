using System.Net;
using Alpha.Common.TokenService;
using Consul;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Refit;

namespace Alpha.Ocelot.Authentication;

public class AlphaAuthenticationDelegatinHandler( ITokenService tokenService, IMemoryCache memCache, 
    ILogger<AlphaAuthenticationDelegatinHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authHeader = request.Headers.Authorization;

        if( authHeader is not null )
        {
            var authString = authHeader.ToString();
            var cacheKey = $"AlphaAuthenticationDelegatinHandler/token/{authString}";
            var cached = memCache.Get<ApiResponse<object>>(cacheKey);

            if(cached is null)
            {
                cached = await tokenService.CheckToken(authString);
                memCache.Set(cacheKey, cached, DateTimeOffset.Now.AddSeconds(60));
            }

            if( !cached.IsSuccessStatusCode )
            {
                logger.LogError("Authentication rejected by Token Service");

                return new HttpResponseMessage {
                    StatusCode = HttpStatusCode.Unauthorized,
                    RequestMessage = request
                };
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}