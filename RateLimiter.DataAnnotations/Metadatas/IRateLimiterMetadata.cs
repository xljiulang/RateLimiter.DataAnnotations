using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 表示速率限制器元数据的接口
    /// </summary>
    public interface IRateLimiterMetadata
    {
        /// <summary>
        /// 获取指定分区键的速率限制分区
        /// </summary>
        /// <param name="key">分区键</param>
        /// <returns>速率限制分区</returns>
        RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key);
    }
}
