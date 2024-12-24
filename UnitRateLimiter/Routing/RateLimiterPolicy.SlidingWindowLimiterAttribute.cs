using System;
using System.Threading.RateLimiting;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    public static partial class RateLimiterPolicy
    {
        /// <summary>
        /// 定义滑动窗口限流策略的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class SlidingWindowLimiterAttribute : Attribute, ISlidingWindowLimiterMetadata
        {
            /// <summary>
            /// 获取滑动窗口限流器的许可限制。
            /// </summary>
            public int PermitLimit { get; }

            /// <summary>
            /// 获取滑动窗口限流器的窗口持续时间（秒）。
            /// </summary>
            public int WindowSeconds { get; }

            /// <summary>
            /// 获取滑动窗口限流器的每个窗口的段数。
            /// </summary>
            public int SegmentsPerWindow { get; }

            /// <summary>
            /// 获取或设置一个值，该值指示限流器是否应自动补充许可。
            /// </summary>
            public bool AutoReplenishment { get; set; } = true;

            /// <summary>
            /// 获取或设置滑动窗口限流器的队列处理顺序。
            /// </summary>
            public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;

            /// <summary>
            /// 获取或设置滑动窗口限流器的队列限制。
            /// </summary>
            public int QueueLimit { get; set; }

            /// <summary>
            /// 初始化 <see cref="SlidingWindowLimiterAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="permitLimit">滑动窗口限流器的许可限制。</param>
            /// <param name="windowSeconds">滑动窗口限流器的窗口持续时间（秒）。</param>
            /// <param name="segmentsPerWindow">滑动窗口限流器的每个窗口的段数。</param>
            public SlidingWindowLimiterAttribute(int permitLimit, int windowSeconds, int segmentsPerWindow)
            {
                PermitLimit = permitLimit;
                WindowSeconds = windowSeconds;
                SegmentsPerWindow = segmentsPerWindow;
            }

            /// <summary>
            /// 获取滑动窗口限流器的选项。
            /// </summary>
            /// <param name="unit">用于获取限流器选项的单位。可以为 null。</param>
            /// <returns>滑动窗口限流器的选项。</returns>
            SlidingWindowRateLimiterOptions ISlidingWindowLimiterMetadata.GetLimiterOptions(string? unit)
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
}