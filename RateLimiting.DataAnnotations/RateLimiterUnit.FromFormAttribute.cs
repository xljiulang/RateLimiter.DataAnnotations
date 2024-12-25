using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Form的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class FromFormAttribute : Attribute, IRateLimiterUnitMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 <see cref="FromFormAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="unitName">表单中用于限流单元的键名。</param>
            public FromFormAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <inheritdoc></inheritdoc>/>
            public virtual ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Form.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
