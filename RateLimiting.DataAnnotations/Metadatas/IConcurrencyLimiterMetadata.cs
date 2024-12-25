using System.Threading.RateLimiting;

namespace RateLimiting.DataAnnotations.Metadatas
{
    /// <summary>
    /// 定义并发限制器元数据的接口。
    /// </summary>
    public interface IConcurrencyLimiterMetadata : IRateLimiterMetadata
    {
        RateLimitPartition<UnitPartitionKey> IRateLimiterMetadata.GetPartition(UnitPartitionKey key)
        {
            return RateLimitPartition.GetConcurrencyLimiter(key, GetLimiterOptions);
        }

        /// <summary>
        /// 获取指定单元的并发限制器选项。
        /// </summary>
        /// <param name="key">单元分区键。</param>
        /// <returns>指定单元的并发限制器选项。</returns>
        ConcurrencyLimiterOptions GetLimiterOptions(UnitPartitionKey key);
    }
}
