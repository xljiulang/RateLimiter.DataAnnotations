using Microsoft.AspNetCore.Http;
using RateLimiter.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiter.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Header的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromHeaderAttribute : Attribute, IRateLimiterUnitMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 <see cref="FromHeaderAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="unitName">用于限流的单元名称。</param>
            public FromHeaderAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <summary>
            /// 根据给定的 HTTP 上下文异步检索用于速率限制的单位标识符。
            /// </summary>
            /// <param name="context">包含请求信息的 HTTP 上下文。</param>
            /// <returns>返回包含单位标识符的 <see cref="ValueTask{TResult}"/> 对象，如果未找到则返回 null。</returns>
            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Headers.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
