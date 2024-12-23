using Microsoft.AspNetCore.RateLimiting;
using System.Linq;
using UnitLimiter.Metadatas;
using UnitLimiter.Policies;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// EndpointConventionBuilder扩展
    /// </summary>
    public static partial class EndpointConventionBuilderExtensions
    {
        /// <summary>
        /// 添加并发限流策略
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static RateLimiterOptions AddUiniLimiterPolicies(this RateLimiterOptions options)
        {
            options.AddPolicy<UnitPartitionKey, ConcurrencyLimiterPolicy>(ConcurrencyLimiterPolicy.PolicyName);
            options.AddPolicy<UnitPartitionKey, SlidingWindowLimiterPolicy>(SlidingWindowLimiterPolicy.PolicyName);
            return options;
        }

        /// <summary>
        /// 添加并发限流策略约定
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEndpointConventionBuilder AddUnitLimiterPolicyConventions(this IEndpointConventionBuilder builder)
        {
            builder.Add(endpoint =>
            {
                var medadata = endpoint.Metadata.OfType<IConcurrencyLimiterMetadata>();
                if (medadata != null)
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
                var medadata = endpoint.Metadata.OfType<ISlidingWindowLimiterMetadata>().FirstOrDefault();
                if (medadata != null)
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
