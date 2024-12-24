using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    public static partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是路由的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromRouteAttribute : Attribute, IUnitRateLimiterMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            public FromRouteAttribute(string unitName)
            {
                UnitName = unitName;
            }

            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.GetRouteValue(UnitName)?.ToString();
                return ValueTask.FromResult(unit);
            }
        }
    }
}
