using Microsoft.AspNetCore.RateLimiting;
using RateLimiter.DataAnnotations.Metadatas;
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
        /// 将单位限流器策略约定添加到 <see cref="IEndpointConventionBuilder"/>。
        /// </summary>
        /// <param name="builder">要添加约定的 <see cref="IEndpointConventionBuilder"/>。</param>
        /// <returns>添加了约定的 <see cref="IEndpointConventionBuilder"/>。</returns>
        public static IEndpointConventionBuilder AddRateLimiterDataAnnotations(this IEndpointConventionBuilder builder)
        {
            builder.Add(endpoint =>
            {
                var policyMetadataCount = endpoint.Metadata.OfType<IRateLimiterPolicyMetadata>().Count();
                if (policyMetadataCount == 0)
                {
                    return;
                }

                if (policyMetadataCount > 1)
                {
                    throw new InvalidOperationException("Only one rate limiter policy can be applied to an endpoint.");
                }

                var attribute = endpoint.Metadata.OfType<EnableRateLimitingAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    endpoint.Metadata.Remove(attribute);
                }

                var policyName = nameof(IRateLimiterPolicyMetadata);
                endpoint.Metadata.Add(new EnableRateLimitingAttribute(policyName));
            });

            return builder;
        }

    }
}
