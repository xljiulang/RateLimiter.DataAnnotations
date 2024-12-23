using System;
using System.Threading.RateLimiting;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    /// <summary>
    /// 表示一个并发限制策略的属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ConcurrencyLimiterPolicyAttribute : Attribute, IConcurrencyLimiterMetadata
    {
        /// <summary>
        /// 获取并发数。
        /// </summary>
        public int PermitLimit { get; }

        /// <summary>
        /// 获取或设置队列限制。
        /// </summary>
        public int QueueLimit { get; set; } = 0;

        /// <summary>
        /// 获取或设置队列处理顺序。
        /// </summary>
        public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;

        /// <summary>
        /// 初始化 <see cref="ConcurrencyLimiterPolicyAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="permitLimit">并发数限制。</param>
        public ConcurrencyLimiterPolicyAttribute(int permitLimit)
        {
            PermitLimit = permitLimit;
        }

     
        ConcurrencyLimiterOptions IConcurrencyLimiterMetadata.GetLimiterOptions(string? unit)
        {
            return new ConcurrencyLimiterOptions
            {
                PermitLimit = PermitLimit,
                QueueLimit = QueueLimit,
                QueueProcessingOrder = QueueProcessingOrder
            };
        }
    }
}
