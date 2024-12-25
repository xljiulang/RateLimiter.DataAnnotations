using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 表示令牌桶限流的元数据接口。
    /// </summary>
    public interface ITokenBucketRateLimiterMetadata : IRateLimiterMetadata
    {
        RateLimitPartition<UnitPartitionKey> IRateLimiterMetadata.GetPartition(UnitPartitionKey key)
        {
            return RateLimitPartition.GetTokenBucketLimiter(key, GetLimiterOptions);
        }

        /// <summary>
        /// 获取限流器选项。
        /// </summary>
        /// <param name="key">单元分区键。</param>
        /// <returns>返回 <see cref="TokenBucketRateLimiterOptions"/> 对象。</returns>
        TokenBucketRateLimiterOptions GetLimiterOptions(UnitPartitionKey key);
    }
}
