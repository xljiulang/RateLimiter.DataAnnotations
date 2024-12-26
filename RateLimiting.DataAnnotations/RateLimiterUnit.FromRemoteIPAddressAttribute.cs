using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是远程IP地址的特性。
        /// </summary>
        public class FromRemoteIPAddressAttribute : RateLimiterUnit
        {
            /// <inheritdoc></inheritdoc>/>
            public override ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var ip) && System.Net.IPAddress.TryParse(ip, out _))
                    return ValueTask.FromResult((string?)ip);

                var unit = context.Connection.RemoteIpAddress?.ToString();
                return ValueTask.FromResult(unit);
            }
        }
    }
}
