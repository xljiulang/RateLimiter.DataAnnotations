using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations.Metadatas
{
    /// <summary>
    /// 表示令牌桶限流策略的元数据接口。
    /// </summary>
    public interface ITokenBucketRateLimiterPolicyMetadata : IRateLimiterPolicyMetadata
    {
        /// <summary>
        /// 获取限流器选项。
        /// </summary>
        /// <param name="unit">单位，可以为空。</param>
        /// <returns>返回 <see cref="TokenBucketRateLimiterOptions"/> 对象。</returns>
        TokenBucketRateLimiterOptions GetLimiterOptions(string? unit);
    }
}
