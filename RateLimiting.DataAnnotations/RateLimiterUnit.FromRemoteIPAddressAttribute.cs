using Microsoft.AspNetCore.Http;
using RateLimiting.DataAnnotations.Metadatas;
using System;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是远程IP地址的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public class FromRemoteIPAddressAttribute : Attribute, IRateLimiterUnitMetadata
        {
            /// <inheritdoc></inheritdoc>/>
            public virtual ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                var unit = context.Connection.RemoteIpAddress?.ToString();
                return ValueTask.FromResult(unit);
            }
        }
    }
}
