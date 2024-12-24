using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Policies
{
    sealed class SlidingWindowLimiterPolicy : UnitRateLimiterPolicy
    {
        public const string PolicyName = "DataAnnotations.SlidingWindowLimiterPolicy";

        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<ISlidingWindowLimiterPolicyMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetSlidingWindowLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
