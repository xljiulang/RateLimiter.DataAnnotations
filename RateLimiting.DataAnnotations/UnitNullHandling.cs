namespace RateLimiting.DataAnnotations
{
    ///<summary>
    /// 表示处理空单元的方式。
    /// </summary>
    public enum UnitNullHandling
    {
        /// <summary>
        /// 当unit值为null时，禁用限流器。
        /// </summary>
        DisableRateLimiter = 0,

        /// <summary>
        /// 当unit值为null时，使用string.Empty单元值进行限流。
        /// </summary>
        TreatAsEmptyUnit = 1,
    }
}
