using System.Threading.RateLimiting;
using UnitLimiter.Metadatas;

namespace UnitLimiter.Policies
{
    /// <summary>
    /// ConcurrencyLimiter策略
    /// </summary> 
    sealed class ConcurrencyLimiterPolicy : UnitLimiterPolicyBase
    {
        /// <summary>
        /// 取策略名
        /// </summary>
        public const string PolicyName = "UnitLimiter.ConcurrencyLimiterPolicy";

        /// <summary>
        /// <inheritdoc/>
        /// </summary> 
        protected override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
        {
            var metadata = key.Endpoint.Metadata.GetMetadata<IConcurrencyLimiterMetadata>();
            return metadata == null
                ? RateLimitPartition.GetNoLimiter(key)
                : RateLimitPartition.GetConcurrencyLimiter(key, k => metadata.GetLimiterOptions(k.Unit));
        }
    }
}
