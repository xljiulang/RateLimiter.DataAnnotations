using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using RateLimiting.DataAnnotations;
using RateLimiting.DataAnnotations.Features;
using RateLimiting.DataAnnotations.Metadatas;
using System.Threading;
using System;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 提供扩展方法以添加速率限制数据注释服务
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加速率限制数据注释服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="rejectedHandler">被限制时的处理</param>
        /// <returns>更新后的服务集合</returns>
        public static IServiceCollection AddRateLimiterDataAnnotations(this IServiceCollection services, Func<OnRejectedContext, CancellationToken, ValueTask>? rejectedHandler = null)
        {
            var policyName = nameof(IRateLimiterMetadata);
            services.AddRateLimiter(o => o.AddPolicy(policyName, GetHttpContextPartition));
            return services;
        }

        /// <summary>
        /// 获取HttpContext的分区
        /// </summary>
        /// <param name="httpContext">HTTP上下文</param>
        /// <returns>单元分区键</returns>
        private static RateLimitPartition<UnitPartitionKey> GetHttpContextPartition(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            var policyMetadata = endpoint.Metadata.GetMetadata<IRateLimiterMetadata>();
            if (policyMetadata == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            var unitFeature = httpContext.Features.Get<IRateLimiterUnitFeature>();
            if (unitFeature == null)
            {
                return policyMetadata.GetPartition(new UnitPartitionKey(endpoint, string.Empty));
            }

            if (unitFeature.Unit == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            return policyMetadata.GetPartition(new UnitPartitionKey(endpoint, unitFeature.Unit));
        }
    }
}
