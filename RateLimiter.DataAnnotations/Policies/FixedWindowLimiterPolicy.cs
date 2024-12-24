using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Policies
{
    sealed class FixedWindowLimiterPolicy : UnitRateLimiterPolicy
    {
        public const string PolicyName = "DataAnnotations.FixedWindowLimiterPolicy";

        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<IFixedWindowLimiterPolicyMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetFixedWindowLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
