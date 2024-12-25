using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 表示固定窗口限流器的元数据。
    /// </summary>
    public interface IFixedWindowRateLimiterMetadata : IRateLimiterMetadata
    {
        RateLimitPartition<UnitPartitionKey> IRateLimiterMetadata.GetPartition(UnitPartitionKey key)
        {
            return RateLimitPartition.GetFixedWindowLimiter(key, GetLimiterOptions);
        }

        /// <summary>
        /// 根据指定的单位获取固定窗口限流器的选项。
        /// </summary>
        /// <param name="key">单元分区键。</param>
        /// <returns>固定窗口限流器的选项。</returns>
        FixedWindowRateLimiterOptions GetLimiterOptions(UnitPartitionKey key);
    }
}
