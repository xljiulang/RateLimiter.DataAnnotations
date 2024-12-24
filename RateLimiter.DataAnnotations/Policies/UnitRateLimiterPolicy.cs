using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using RateLimiter.DataAnnotations.Middlewares;
using System;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace RateLimiter.DataAnnotations.Policies
{
    abstract partial class UnitRateLimiterPolicy : IRateLimiterPolicy<UnitPartitionKey>
    {

        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => RejectedAsync;


        protected virtual ValueTask RejectedAsync(OnRejectedContext context, CancellationToken cancellationToken)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return ValueTask.CompletedTask;
        }

        public RateLimitPartition<UnitPartitionKey> GetPartition(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            var unitFeature = httpContext.Features.Get<IUnitFeature>();
            if (unitFeature == null)
            {
                return GetPartition(new UnitPartitionKey(endpoint, string.Empty));
            }

            if (unitFeature.Unit == null)
            {
                return RateLimitPartition.GetNoLimiter(UnitPartitionKey.None);
            }

            return GetPartition(new UnitPartitionKey(endpoint, unitFeature.Unit));
        }

        protected abstract RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key);
    }
}
