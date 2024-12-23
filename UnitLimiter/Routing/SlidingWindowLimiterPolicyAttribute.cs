using UnitLimiter.Metadatas;
using System;
using System.Threading.RateLimiting;

namespace UnitLimiter.Routing
{
    /// <summary>
    /// 滑动窗口限流策略特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SlidingWindowLimiterPolicyAttribute : Attribute, ISlidingWindowLimiterMetadata
    {
        /// <summary>
        /// 获取请求次数限制
        /// </summary>
        public int PermitLimit { get; }

        /// <summary>
        /// 窗口大小的秒数
        /// 默认一分钟
        /// </summary>
        public int WindowSeconds { get; }

        public int SegmentsPerWindow { get; set; }

        public bool AutoReplenishment { get; set; } = true;

        public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;

        public int QueueLimit { get; set; }

        /// <summary>
        /// 滑动窗口限流策略特性
        /// </summary>
        /// <param name="permitLimit">请求次数限制</param>
        /// <param name="windowSeconds"></param>
        public SlidingWindowLimiterPolicyAttribute(int permitLimit, int windowSeconds)
        {
            PermitLimit = permitLimit;
            WindowSeconds = windowSeconds;
        }

        SlidingWindowRateLimiterOptions ISlidingWindowLimiterMetadata.GetLimiterOptions(string? userId)
        {
            return new SlidingWindowRateLimiterOptions
            {
                PermitLimit = PermitLimit,
                Window = TimeSpan.FromSeconds(WindowSeconds),
                SegmentsPerWindow = SegmentsPerWindow,
                AutoReplenishment = AutoReplenishment,
                QueueLimit = QueueLimit,
                QueueProcessingOrder = QueueProcessingOrder
            };
        }
    }
}
