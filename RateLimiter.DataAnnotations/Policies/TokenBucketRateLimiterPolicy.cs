using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Policies
{
    sealed class TokenBucketRateLimiterPolicy : UnitRateLimiterPolicy
    {
        public const string PolicyName = "DataAnnotations.TokenBucketRateLimiterPolicy";

        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<ITokenBucketRateLimiterPolicyMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetTokenBucketLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
