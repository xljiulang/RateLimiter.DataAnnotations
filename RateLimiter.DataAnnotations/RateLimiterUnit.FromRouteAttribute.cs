using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RateLimiter.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiter.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是路由的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromRouteAttribute : Attribute, IRateLimiterUnitMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 FromRouteAttribute 类的新实例。
            /// </summary>
            /// <param name="unitName">单元的名称。</param>
            public FromRouteAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <summary>
            /// 根据给定的 HTTP 上下文异步检索用于速率限制的单位标识符。
            /// </summary>
            /// <param name="context">包含请求信息的 HTTP 上下文。</param>
            /// <returns>返回 null 表示无限制。</returns>
            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.GetRouteValue(UnitName)?.ToString();
                return ValueTask.FromResult(unit);
            }
        }
    }
}
