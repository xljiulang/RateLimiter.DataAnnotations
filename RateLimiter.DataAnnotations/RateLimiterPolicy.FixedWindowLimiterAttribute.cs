using RateLimiter.DataAnnotations.Metadatas;
using System;
using System.Threading.RateLimiting;

namespace RateLimiter.DataAnnotations
{
    public static partial class RateLimiterPolicy
    {
        /// <summary>
        /// 表示一个固定窗口限流器的属性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FixedWindowLimiterAttribute : Attribute, IFixedWindowLimiterPolicyMetadata
        {
            /// <summary>
            /// 获取每个窗口允许的请求数。
            /// </summary>
            public int PermitLimit { get; }

            /// <summary>
            /// 获取窗口的持续时间（以秒为单位）。
            /// </summary>
            public int WindowSeconds { get; }

            /// <summary>
            /// 获取或设置一个值，该值指示是否自动补充令牌。
            /// </summary>
            public bool AutoReplenishment { get; set; } = true;

            /// <summary>
            /// 获取或设置队列处理顺序。
            /// </summary>
            public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;

            /// <summary>
            /// 获取或设置队列的最大长度。
            /// </summary>
            public int QueueLimit { get; set; }

            /// <summary>
            /// 初始化 <see cref="FixedWindowLimiterAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="permitLimit">每个窗口允许的请求数。</param>
            /// <param name="windowSeconds">窗口的持续时间（以秒为单位）。</param>
            public FixedWindowLimiterAttribute(int permitLimit, int windowSeconds)
            {
                PermitLimit = permitLimit;
                WindowSeconds = windowSeconds;
            }

           
            public FixedWindowRateLimiterOptions GetLimiterOptions(UnitPartitionKey key)
            {
                return new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = this.AutoReplenishment,
                    PermitLimit = this.PermitLimit,
                    QueueLimit = this.QueueLimit,
                    QueueProcessingOrder = this.QueueProcessingOrder,
                    Window = TimeSpan.FromSeconds(this.WindowSeconds)
                };
            }
        }
    }
}