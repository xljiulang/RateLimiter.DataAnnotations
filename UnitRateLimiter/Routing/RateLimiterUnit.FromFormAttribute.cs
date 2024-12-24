using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    public static partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Form的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromFormAttribute : Attribute, IUnitRateLimiterMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            public FromFormAttribute(string unitName)
            {
                UnitName = unitName;
            }

            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Form.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
