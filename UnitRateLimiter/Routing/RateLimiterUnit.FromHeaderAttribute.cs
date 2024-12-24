using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    public static partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Header的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromHeaderAttribute : Attribute, IUnitRateLimiterMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            public FromHeaderAttribute(string unitName)
            {
                UnitName = unitName;
            }

            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Headers.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
