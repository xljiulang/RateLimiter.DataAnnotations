using System.Threading.RateLimiting;

namespace UnitLimiter.Metadatas
{
    /// <summary>
    /// SlidingWindowLimiter的元数据
    /// </summary>
    public interface ISlidingWindowLimiterMetadata
    {
        SlidingWindowRateLimiterOptions GetLimiterOptions(string? unit);
    }
}
