using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是路由的特性。
        /// </summary>
        public class FromRouteAttribute : RateLimiterUnit
        {
            /// <summary>
            /// 获取路由中表示限流单元的键名。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 FromRouteAttribute 类的新实例。
            /// </summary>
            /// <param name="unitName">路由中表示限流单元的键名</param>
            public FromRouteAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <inheritdoc></inheritdoc>/>
            public override ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.GetRouteValue(UnitName)?.ToString();
                return ValueTask.FromResult(unit);
            }
        }
    }
}
