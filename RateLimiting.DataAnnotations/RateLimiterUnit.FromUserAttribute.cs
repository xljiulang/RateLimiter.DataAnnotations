using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是<see cref="ClaimsPrincipal"/>类型的用户的特性。
        /// </summary>
        public class FromUserAttribute : RateLimiterUnit
        {
            /// <summary>
            /// 获取用户中表示限流单元的ChiamType。
            /// </summary>
            public string UnitName { get; }

            /// <summary>
            /// 初始化 <see cref="FromUserAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="unitName">用户中表示限流单元的ChiamType。</param>
            public FromUserAttribute(string unitName)
            {
                UnitName = unitName;
            }

            /// <inheritdoc></inheritdoc>/>
            public override ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.User.FindFirstValue(UnitName);
                return ValueTask.FromResult(unit);
            }
        }
    }
}
