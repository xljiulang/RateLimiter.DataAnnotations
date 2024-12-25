using Microsoft.AspNetCore.Http;
using RateLimiter.DataAnnotations.Features;
using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 提供扩展方法以便在 <see cref="IApplicationBuilder"/> 中使用速率限制中间件。
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用RateLimiterDataAnnotations的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRateLimiterDataAnnotations(this IApplicationBuilder app)
        {
            return app.Use(next => context => InvokeAsync(context, next));
        }

        private static async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var unitFeature = context.Features.Get<IRateLimiterUnitFeature>();
            if (unitFeature == null)
            {
                var unitMetadata = context.GetEndpoint()?.Metadata.GetMetadata<IRateLimiterUnitMetadata>();
                if (unitMetadata != null)
                {
                    var unit = await unitMetadata.GetUnitAsync(context);
                    unitFeature = new RateLimiterUnitFeature(unit);
                    context.Features.Set(unitFeature);
                }
            }

            await next(context);
        }


        private sealed class RateLimiterUnitFeature(string? unit) : IRateLimiterUnitFeature
        {
            public string? Unit { get; } = unit;
        }
    }
}
