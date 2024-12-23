using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using UnitLimiter.Middlewares;

namespace UnitLimiter.Policies
{
    /// <summary>
    /// Endpoint限流策略基类
    /// </summary>
    abstract partial class UnitLimiterPolicyBase : IRateLimiterPolicy<UnitPartitionKey>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => RejectedAsync;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private ValueTask RejectedAsync(OnRejectedContext context, CancellationToken cancellationToken)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public RateLimitPartition<UnitPartitionKey> GetPartition(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            var unitFeature = httpContext.Features.Get<ILimiterUnitFeature>();
            return GetPartition(new UnitPartitionKey(endpoint, unitFeature?.Unit));
        }

        /// <summary>
        /// 获取RateLimitPartition
        /// </summary>
        /// <param name="key"></param> 
        /// <returns></returns>
        protected abstract RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key);
    }
}
