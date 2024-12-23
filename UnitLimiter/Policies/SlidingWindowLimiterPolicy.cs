using System.Threading.RateLimiting;
using UnitLimiter.Metadatas;

namespace UnitLimiter.Policies
{
    /// <summary>
    /// SlidingWindowLimiter策略
    /// </summary> 
    sealed class SlidingWindowLimiterPolicy : UnitLimiterPolicyBase
    {
        /// <summary>
        /// 策略名
        /// </summary>
        public const string PolicyName = "UnitLimiter.SlidingWindowLimiterPolicy";

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<ISlidingWindowLimiterMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetSlidingWindowLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
