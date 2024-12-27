using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是Query的特性。
        /// </summary>
        public class FromQueryAttribute : RateLimiterUnit
        {
            /// <summary>
            /// 获取Query中表示限流单元的键名。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 限流单元单位来源是Query的特性。
            /// </summary>
            /// <param name="unitName">Query中表示限流单元的键名。</param>
            public FromQueryAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <inheritdoc></inheritdoc>/>
            public override ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Request.Query.TryGetValue(UnitName, out var unitValue) ? (string?)unitValue : null;
                return ValueTask.FromResult(unit);
            }
        }
    }
}
