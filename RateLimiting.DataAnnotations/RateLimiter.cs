using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.RateLimiting;

namespace RateLimiting.DataAnnotations
{
    /// <summary>
    /// RateLimiter 类的部分定义，包含限流器的特性。
    /// <para>◆ 必须指定一种限流器才会限流。</para>
    /// <para>◆ 不支持同时指定多种限流器。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract partial class RateLimiter : Attribute, IRateLimiterMetadata
    {
        /// <summary>
        /// 获取指定分区键的速率限制分区
        /// </summary>
        /// <param name="key">分区键</param>
        /// <returns>速率限制分区</returns>
        public abstract RateLimitPartition<UnitPartitionKey> GetPartition(UnitPartitionKey key);
    }
}
