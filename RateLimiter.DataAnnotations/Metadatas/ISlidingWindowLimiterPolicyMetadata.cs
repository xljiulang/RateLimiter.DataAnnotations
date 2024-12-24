using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 与滑动窗口限流器相关的元数据接口。
    /// </summary>
    public interface ISlidingWindowLimiterPolicyMetadata : IRateLimiterPolicyMetadata
    {
        RateLimitPartition<UnitPartitionKey> IRateLimiterPolicyMetadata.GetPartition(UnitPartitionKey key)
        {
            return RateLimitPartition.GetSlidingWindowLimiter(key, k => GetLimiterOptions(k.Unit));
        }

        /// <summary>
        /// 根据指定的单位获取滑动窗口限流器的选项。
        /// </summary>
        /// <param name="unit">要获取限流器选项的单位。可以为 null。</param>
        /// <returns>滑动窗口限流器的选项。</returns>
        SlidingWindowRateLimiterOptions GetLimiterOptions(string? unit);
    }
}
