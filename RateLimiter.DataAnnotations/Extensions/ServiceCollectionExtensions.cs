using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RateLimiter.DataAnnotations;
using RateLimiter.DataAnnotations.Features;
using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.RateLimiting;

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
        /// <returns>更新后的服务集合</returns>
        public static IServiceCollection AddRateLimiterDataAnnotations(this IServiceCollection services)
        {
            var policyName = nameof(IRateLimiterPolicyMetadata);
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

            var policyMetadata = endpoint.Metadata.GetMetadata<IRateLimiterPolicyMetadata>();
            if (policyMetadata == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            var unitFeature = httpContext.Features.Get<IUnitFeature>();
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
