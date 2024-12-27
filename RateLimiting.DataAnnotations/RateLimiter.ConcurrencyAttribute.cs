﻿using System.Threading.RateLimiting;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiter
    {
        /// <summary>
        /// 表示一个并发限制策略的属性。
        /// </summary>
        public class ConcurrencyAttribute : RateLimiter
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
            /// 初始化 <see cref="ConcurrencyAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="permitLimit">并发数限制。</param>
            public ConcurrencyAttribute(int permitLimit)
            {
                PermitLimit = permitLimit;
            }

            /// <inheritdoc></inheritdoc>/>
            public sealed override RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key)
            {
                return RateLimitPartition.GetConcurrencyLimiter(key, GetLimiterOptions);
            }

            /// <summary>
            /// 获取指定单元的并发限制器选项。
            /// </summary>
            /// <param name="key">单元分区键。</param>
            /// <returns>指定单元的并发限制器选项。</returns>
            protected virtual ConcurrencyLimiterOptions GetLimiterOptions(UnitPartitionKey key)
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
}