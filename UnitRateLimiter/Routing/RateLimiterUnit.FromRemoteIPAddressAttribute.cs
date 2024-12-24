using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    public static partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是远程IP地址的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromRemoteIPAddressAttribute : Attribute, IUnitRateLimiterMetadata
        {
            public ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Connection.RemoteIpAddress?.ToString();
                return ValueTask.FromResult(unit);
            }
        }
    }
}
