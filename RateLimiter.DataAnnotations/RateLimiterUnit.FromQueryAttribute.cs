using Microsoft.AspNetCore.Http;
using RateLimiter.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiter.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Query的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromQueryAttribute : Attribute, IRateLimiterUnitMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 <see cref="FromQueryAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="unitName">查询字符串中表示单元名称的键。</param>
            public FromQueryAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <summary>
            /// 根据给定的 HTTP 上下文异步检索用于速率限制的单位标识符。
            /// </summary>
            /// <param name="context">包含请求信息的 HTTP 上下文。</param>
            /// <returns>返回查询字符串中与 <see cref="UnitName"/> 对应的值，如果不存在则返回 null。</returns>
            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Query.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
