namespace RateLimiting.DataAnnotations.Features
{
    /// <summary>
    /// 表示限速的单元特性。
    /// </summary>
    public interface IRateLimiterUnitFeature
    {
        /// <summary>
        /// 获取限流单元的值
        /// </summary>
        string? Unit { get; }
    }
}
