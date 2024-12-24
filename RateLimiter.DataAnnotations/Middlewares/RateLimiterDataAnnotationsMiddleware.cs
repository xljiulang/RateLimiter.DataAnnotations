using Microsoft.AspNetCore.Http;
using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.Tasks;

namespace RateLimiter.DataAnnotations.Middlewares
{
    sealed class RateLimiterDataAnnotationsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var unitFeature = context.Features.Get<IUnitFeature>();
            if (unitFeature == null)
            {
                var metadata = context.GetEndpoint()?.Metadata.GetMetadata<IRateLimiterUnitMetadata>();
                if (metadata != null)
                {
                    var unit = await metadata.GetUnitAsync(context);
                    unitFeature = new UnitFeature(unit);
                    context.Features.Set(unitFeature);
                }
            }
            await next(context);
        }


        private sealed class UnitFeature(string? unit) : IUnitFeature
        {
            public string? Unit { get; } = unit;
        }
    }
}
