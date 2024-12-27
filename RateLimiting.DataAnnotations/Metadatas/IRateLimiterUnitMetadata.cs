using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations.Metadatas
{
    /// <summary>
    /// 提供与单位速率限制相关的元数据的接口。
    /// </summary>
    public interface IRateLimiterUnitMetadata
    {
        /// <summary>
        /// 获取处理null单元的方式。
        /// </summary>
        UnitNullHandling UnitNullHandling { get; }

        /// <summary>
        /// 根据给定的 HTTP 上下文异步检索用于速率限制的单位标识符。
        /// </summary>
        /// <param name="context">包含请求信息的 HTTP 上下文。</param>
        /// <returns></returns>
        ValueTask<string?> GetUnitAsync(HttpContext context);
    }
}
