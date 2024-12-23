using Microsoft.AspNetCore.RateLimiting;
using System.Linq;
using UnitRateLimiter.Metadatas;
using UnitRateLimiter.Policies;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 提供用于配置端点上的限流策略和约定的扩展方法。
    /// </summary>
    public static class EndpointConventionBuilderExtensions
    {
        /// <summary>
        /// 将单位限流器策略添加到 <see cref="RateLimiterOptions"/>。
        /// </summary>
        /// <param name="options">要添加策略的 <see cref="RateLimiterOptions"/>。</param>
        /// <returns>添加了策略的 <see cref="RateLimiterOptions"/>。</returns>
        public static RateLimiterOptions AddUnitRateLimiterPolicies(this RateLimiterOptions options)
        {
            options.AddPolicy<UnitPartitionKey, ConcurrencyLimiterPolicy>(ConcurrencyLimiterPolicy.PolicyName);
            options.AddPolicy<UnitPartitionKey, SlidingWindowLimiterPolicy>(SlidingWindowLimiterPolicy.PolicyName);
            return options;
        }

        /// <summary>
        /// 将单位限流器策略约定添加到 <see cref="IEndpointConventionBuilder"/>。
        /// </summary>
        /// <param name="builder">要添加约定的 <see cref="IEndpointConventionBuilder"/>。</param>
        /// <returns>添加了约定的 <see cref="IEndpointConventionBuilder"/>。</returns>
        public static IEndpointConventionBuilder AddUnitRateLimiterPolicyConventions(this IEndpointConventionBuilder builder)
        {
            builder.Add(endpoint =>
            {
                var metadata = endpoint.Metadata.OfType<IConcurrencyLimiterMetadata>();
                if (metadata != null)
                {
                    var attribute = endpoint.Metadata.OfType<EnableRateLimitingAttribute>().FirstOrDefault();
                    if (attribute != null)
                    {
                        endpoint.Metadata.Remove(attribute);
                    }
                    endpoint.Metadata.Add(new EnableRateLimitingAttribute(ConcurrencyLimiterPolicy.PolicyName));
                }
            });

            builder.Add(endpoint =>
            {
                var metadata = endpoint.Metadata.OfType<ISlidingWindowLimiterMetadata>().FirstOrDefault();
                if (metadata != null)
                {
                    var attribute = endpoint.Metadata.OfType<EnableRateLimitingAttribute>().FirstOrDefault();
                    if (attribute != null)
                    {
                        endpoint.Metadata.Remove(attribute);
                    }
                    endpoint.Metadata.Add(new EnableRateLimitingAttribute(SlidingWindowLimiterPolicy.PolicyName));
                }
            });
            return builder;
        }
    }
}
