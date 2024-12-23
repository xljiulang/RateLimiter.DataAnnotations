using System.Threading.RateLimiting;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Policies
{
    sealed class SlidingWindowLimiterPolicy : UnitRateLimiterPolicy
    {
        public const string PolicyName = "UnitLimiter.SlidingWindowLimiterPolicy";

        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<ISlidingWindowLimiterMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetSlidingWindowLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
