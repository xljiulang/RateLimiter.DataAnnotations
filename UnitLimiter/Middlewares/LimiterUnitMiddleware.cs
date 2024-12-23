using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UnitLimiter.Metadatas;

namespace UnitLimiter.Middlewares
{
    sealed class LimiterUnitMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var feature = context.Features.Get<ILimiterUnitFeature>();
            if (feature == null)
            {
                var unit = await GetUnitAsync(context);
                feature = new LimiterUnitFeature(unit);
                context.Features.Set(feature);
            }
            await next(context);
        }

        private static async ValueTask<string?> GetUnitAsync(HttpContext context)
        {
            var metadata = context.GetEndpoint()?.Metadata.GetMetadata<ILimiterUnitMetadataProvider>();
            return metadata == null ? null : await metadata.GetUnitAsync(context);
        }
    }
}
