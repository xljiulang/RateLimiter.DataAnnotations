using System.Threading.RateLimiting;

namespace UnitLimiter.Metadatas
{
    /// <summary>
    /// ConcurrencyLimiter的元数据
    /// </summary>
    public interface IConcurrencyLimiterMetadata
    {
        ConcurrencyLimiterOptions GetLimiterOptions(string? unit);
    }
}
