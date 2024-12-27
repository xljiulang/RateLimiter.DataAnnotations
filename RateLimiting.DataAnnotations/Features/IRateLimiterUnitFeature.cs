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

        /// <summary>
        /// 获取处理null单元的方式
        /// </summary>
        UnitNullHandling UnitNullHandling { get; }
    }
}
