using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.RateLimiting;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiter
    {
        /// <summary>
        /// 定义滑动窗口限流器的特性。
        /// </summary>
        public class SlidingWindowAttribute : RateLimiter, ISlidingWindowRateLimiterMetadata
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
            /// 初始化 <see cref="SlidingWindowAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="permitLimit">滑动窗口限流器的许可限制。</param>
            /// <param name="windowSeconds">滑动窗口限流器的窗口持续时间（秒）。</param>
            /// <param name="segmentsPerWindow">滑动窗口限流器的每个窗口的段数。</param>
            public SlidingWindowAttribute(int permitLimit, int windowSeconds, int segmentsPerWindow)
            {
                PermitLimit = permitLimit;
                WindowSeconds = windowSeconds;
                SegmentsPerWindow = segmentsPerWindow;
            }

            /// <inheritdoc></inheritdoc>/>
            public virtual SlidingWindowRateLimiterOptions GetLimiterOptions(UnitPartitionKey key)
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