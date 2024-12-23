using System;
using System.Threading.RateLimiting;
using UnitLimiter.Metadatas;

namespace UnitLimiter.Routing
{
    /// <summary>
    /// 并发限流策略特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ConcurrencyLimiterPolicyAttribute : Attribute, IConcurrencyLimiterMetadata
    {
        /// <summary>
        /// 获取并发数
        /// </summary>
        public int PermitLimit { get; }

        public int QueueLimit { get; set; } = 0;

        public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;

        /// <summary>
        /// 并发限流特性
        /// </summary>
        /// <param name="permitLimit">并发数</param>
        public ConcurrencyLimiterPolicyAttribute(int permitLimit)
        {
            PermitLimit = permitLimit;
        }

        ConcurrencyLimiterOptions IConcurrencyLimiterMetadata.GetLimiterOptions(string? userId)
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
