using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Header的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class FromHeaderAttribute : Attribute, IRateLimiterUnitMetadata
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

            /// <inheritdoc></inheritdoc>/>
            public virtual ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Headers.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
