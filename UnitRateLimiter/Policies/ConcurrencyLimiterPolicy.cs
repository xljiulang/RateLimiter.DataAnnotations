using System.Threading.RateLimiting;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Policies
{
    sealed class ConcurrencyLimiterPolicy : UnitRateLimiterPolicy
    {       
        public const string PolicyName = "UnitLimiter.ConcurrencyLimiterPolicy";

        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<IConcurrencyLimiterMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetConcurrencyLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
