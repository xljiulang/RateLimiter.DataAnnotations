using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Header的特性。
        /// </summary>
        public class FromHeaderAttribute : RateLimiterUnit
        {
            /// <summary>
            /// 获取请求头中表示限流单元的键名。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 <see cref="FromHeaderAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="unitName">请求头中表示限流单元的键名。</param>
            public FromHeaderAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <inheritdoc></inheritdoc>/>
            public override ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Headers.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
