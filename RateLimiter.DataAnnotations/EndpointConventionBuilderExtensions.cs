using Microsoft.AspNetCore.RateLimiting;
using RateLimiter.DataAnnotations.Metadatas;
using RateLimiter.DataAnnotations.Policies;
using System;
using System.Linq;

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
        public static RateLimiterOptions AddRateLimiterDataAnnotations(this RateLimiterOptions options)
        {
            options.AddPolicy<UnitPartitionKey, ConcurrencyLimiterPolicy>(ConcurrencyLimiterPolicy.PolicyName);
            options.AddPolicy<UnitPartitionKey, FixedWindowLimiterPolicy>(FixedWindowLimiterPolicy.PolicyName);
            options.AddPolicy<UnitPartitionKey, SlidingWindowLimiterPolicy>(SlidingWindowLimiterPolicy.PolicyName);
            return options;
        }

        /// <summary>
        /// 将单位限流器策略约定添加到 <see cref="IEndpointConventionBuilder"/>。
        /// </summary>
        /// <param name="builder">要添加约定的 <see cref="IEndpointConventionBuilder"/>。</param>
        /// <returns>添加了约定的 <see cref="IEndpointConventionBuilder"/>。</returns>
        public static IEndpointConventionBuilder AddRateLimiterDataAnnotations(this IEndpointConventionBuilder builder)
        {
            builder.Add(endpoint =>
            {
                var policyMetadatas = endpoint.Metadata.OfType<IRateLimiterPolicyMetadata>().ToArray();
                if (policyMetadatas.Length > 1)
                {
                    throw new InvalidOperationException("Only one rate limiter policy can be applied to an endpoint.");
                }

                var policyMetadata = policyMetadatas.FirstOrDefault();
                if (policyMetadata != null)
                {
                    var policyName = GetPolicyName(policyMetadata);
                    var attribute = endpoint.Metadata.OfType<EnableRateLimitingAttribute>().FirstOrDefault();
                    if (attribute == null || attribute.PolicyName != policyName)
                    {
                        if (attribute != null)
                        {
                            endpoint.Metadata.Remove(attribute);
                        }
                        endpoint.Metadata.Add(new EnableRateLimitingAttribute(policyName));
                    }
                }
            });

            return builder;
        }

        private static string GetPolicyName(IRateLimiterPolicyMetadata policyMetadata)
        {
            return policyMetadata switch
            {
                IConcurrencyLimiterPolicyMetadata => ConcurrencyLimiterPolicy.PolicyName,
                IFixedWindowLimiterPolicyMetadata => FixedWindowLimiterPolicy.PolicyName,
                ISlidingWindowLimiterPolicyMetadata => SlidingWindowLimiterPolicy.PolicyName,
                _ => throw new InvalidOperationException("Unsupported rate limiter policy metadata."),
            };
        }
    }
}
