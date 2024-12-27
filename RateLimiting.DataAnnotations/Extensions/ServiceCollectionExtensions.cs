using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using RateLimiting.DataAnnotations;
using RateLimiting.DataAnnotations.Features;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading;
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
        /// <returns>更新后的服务集合</returns>
        public static IServiceCollection AddRateLimiterDataAnnotations(this IServiceCollection services)
        {
            var policyName = nameof(IRateLimiterMetadata);
            return services.AddRateLimiter(o => o.AddPolicy(policyName, GetRateLimitPartition));
        }

        /// <summary>
        /// 添加速率限制数据注释服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="rejectedHandler">被限制时的处理</param>
        /// <returns>更新后的服务集合</returns>
        public static IServiceCollection AddRateLimiterDataAnnotations(this IServiceCollection services, Func<OnRejectedContext, CancellationToken, ValueTask>? rejectedHandler = null)
        {
            return services.AddRateLimiterDataAnnotations().AddRateLimiter(o => o.OnRejected = rejectedHandler);
        }

        /// <summary>
        /// 获取HttpContext的分区
        /// </summary>
        /// <param name="httpContext">HTTP上下文</param>
        /// <returns>单元分区键</returns>
        private static RateLimitPartition<UnitPartitionKey> GetRateLimitPartition(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
            {
                return NoLimiter();
            }

            var limiterMetadata = endpoint.Metadata.GetMetadata<IRateLimiterMetadata>();
            if (limiterMetadata == null)
            {
                return NoLimiter();
            }

            var unitFeature = httpContext.Features.Get<IRateLimiterUnitFeature>();
            if (unitFeature == null)
            {
                return UnitLimiter(string.Empty);
            }

            var unit = unitFeature.Unit;
            if (unit == null)
            {
                if (unitFeature.UnitNullHandling == UnitNullHandling.NoLimiter)
                {
                    return NoLimiter();
                }

                if (unitFeature.UnitNullHandling == UnitNullHandling.EmptyUnitLimiter)
                {
                    return UnitLimiter(string.Empty);
                }

                throw new NotSupportedException($"{nameof(UnitNullHandling)} value is not supported.");
            }

            return UnitLimiter(unit);



            static RateLimitPartition<UnitPartitionKey> NoLimiter()
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            RateLimitPartition<UnitPartitionKey> UnitLimiter(string unit)
            {
                var partitionKey = new UnitPartitionKey(endpoint, unit);
                return limiterMetadata.GetPartition(partitionKey);
            }
        }
    }
}
