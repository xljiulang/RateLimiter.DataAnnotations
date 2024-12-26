using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Features;
using RateLimiting.DataAnnotations.Metadatas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 提供扩展方法以便在 <see cref="IApplicationBuilder"/> 中使用速率限制中间件。
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用限流注解的中间件
        /// <para>◆ 必此中间件必须在 app.UseRateLimiter()之前</para> 
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
                var metadatas = context.GetEndpoint()?.Metadata.GetOrderedMetadata<IRateLimiterUnitMetadata>();
                if (metadatas?.Count > 0)
                {
                    var unit = await GetUnitAsync(metadatas, context);
                    unitFeature = new RateLimiterUnitFeature(unit);
                    context.Features.Set(unitFeature);
                }
            }

            await next(context);
        }

        private static async Task<string?> GetUnitAsync(IReadOnlyList<IRateLimiterUnitMetadata> metadatas, HttpContext context)
        {
            if (metadatas.Count == 1)
                return await metadatas[0].GetUnitAsync(context);

            var units = await Task.WhenAll(metadatas.Select(async x => await x.GetUnitAsync(context)));
            var unit = string.Join(":", units);
            return unit;
        }

        private sealed class RateLimiterUnitFeature(string? unit) : IRateLimiterUnitFeature
        {
            public string? Unit { get; } = unit;
        }
    }
}
