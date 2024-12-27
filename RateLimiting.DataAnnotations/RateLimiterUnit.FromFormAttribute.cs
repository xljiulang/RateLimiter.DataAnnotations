using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Form的特性。
        /// </summary>
        public class FromFormAttribute : RateLimiterUnit
        {
            /// <summary>
            /// 获取表单中表示限流单元的键名。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 限流单元单位来源是Form的特性。
            /// </summary>
            /// <param name="unitName">表单中表示限流单元的键名。</param>
            public FromFormAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <inheritdoc></inheritdoc>/>
            public override ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Form.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
