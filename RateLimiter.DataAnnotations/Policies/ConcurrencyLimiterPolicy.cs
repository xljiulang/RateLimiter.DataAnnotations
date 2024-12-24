using RateLimiter.DataAnnotations.Metadatas;
using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Policies
{
    sealed class ConcurrencyLimiterPolicy : UnitRateLimiterPolicy
    {       
        public const string PolicyName = "DataAnnotations.ConcurrencyLimiterPolicy";

        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<IConcurrencyLimiterPolicyMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetConcurrencyLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
