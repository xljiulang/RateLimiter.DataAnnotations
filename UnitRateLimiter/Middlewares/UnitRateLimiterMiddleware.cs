using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Middlewares
{
    sealed class UnitRateLimiterMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var unitFeature = context.Features.Get<IUnitFeature>();
            if (unitFeature == null)
            {
                var metadata = context.GetEndpoint()?.Metadata.GetMetadata<IUnitRateLimiterMetadata>();
                if (metadata != null)
                {
                    var unit = await metadata.GetUnitAsync(context);
                    unitFeature = new UnitFeature(unit);
                    context.Features.Set(unitFeature);
                }
            }
            await next(context);
        }
    }
}
