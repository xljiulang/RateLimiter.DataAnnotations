using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Query的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class FromQueryAttribute : Attribute, IRateLimiterUnitMetadata
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

            /// <inheritdoc></inheritdoc>/>
            public virtual ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Query.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
