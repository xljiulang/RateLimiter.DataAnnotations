using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 定义并发限制器元数据的接口。
    /// </summary>
    public interface IConcurrencyLimiterPolicyMetadata : IRateLimiterPolicyMetadata
    {
        RateLimitPartition<UnitPartitionKey> IRateLimiterPolicyMetadata.GetPartition(UnitPartitionKey key)
        {
            return RateLimitPartition.GetConcurrencyLimiter(key, k => GetLimiterOptions(k.Unit));
        }

        /// <summary>
        /// 获取指定单元的并发限制器选项。
        /// </summary>
        /// <param name="unit">要获取限制器选项的单元。</param>
        /// <returns>指定单元的并发限制器选项。</returns>
        ConcurrencyLimiterOptions GetLimiterOptions(string? unit);
    }
}
