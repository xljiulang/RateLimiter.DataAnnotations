using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    /// <summary>
    /// RateLimiterUnit 类的部分定义，包含限流单元来源的特性。
    /// <para>◆ 此特性缺省时表示对所有请求进行限流。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract partial class RateLimiterUnit : Attribute, IRateLimiterUnitMetadata
    {
        /// <summary>
        /// 获取处理null单元的方式。
        /// 默认为UnitNullHandling.NoLimiter
        /// </summary>
        public UnitNullHandling UnitNullHandling { get; set; }

        /// <inheritdoc></inheritdoc>/>
        public abstract ValueTask<string?> GetUnitAsync(HttpContext context);
    }
}
