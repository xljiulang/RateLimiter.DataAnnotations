using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 表示固定窗口限流器策略的元数据。
    /// </summary>
    public interface IFixedWindowLimiterPolicyMetadata : IRateLimiterPolicyMetadata
    {
        RateLimitPartition<UnitPartitionKey> IRateLimiterPolicyMetadata.GetPartition(UnitPartitionKey key)
        {
            return RateLimitPartition.GetFixedWindowLimiter(key, k => GetLimiterOptions(k.Unit));
        }

        /// <summary>
        /// 根据指定的单位获取固定窗口限流器的选项。
        /// </summary>
        /// <param name="unit">要获取限流器选项的单位。可以为 null。</param>
        /// <returns>固定窗口限流器的选项。</returns>
        FixedWindowRateLimiterOptions GetLimiterOptions(string? unit);
    }
}
