using System;

namespace RateLimiting.DataAnnotations
{
    /// <summary>
    /// RateLimiter 类的部分定义，包含限流器的特性。
    /// <para>◆ 必须指定一种限流器才会限流。</para>
    /// <para>◆ 不支持同时指定多种限流器。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract partial class RateLimiter : Attribute
    {
    }
}
