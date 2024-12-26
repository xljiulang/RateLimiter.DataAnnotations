using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    /// <summary>
    /// RateLimiterUnit 类的部分定义，包含限流单元来源的特性。
    /// <para>◆ 此特性缺省时表示对所有请求进行限流。</para>
    /// <para>◆ 此特性获取到的Unit值为null时将取消限流。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract partial class RateLimiterUnit : Attribute, IRateLimiterUnitMetadata
    {
        /// <inheritdoc></inheritdoc>/>
        public abstract ValueTask<string?> GetUnitAsync(HttpContext context);
    }
}
