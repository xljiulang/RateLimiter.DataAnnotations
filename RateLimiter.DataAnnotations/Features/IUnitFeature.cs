namespace RateLimiter.DataAnnotations.Features
{
    /// <summary>
    /// 表示限速的单元特性。
    /// </summary>
    public interface IUnitFeature
    {
        /// <summary>
        /// 获取单元的值
        /// </summary>
        string? Unit { get; }
    }
}
