using RateLimiter.DataAnnotations.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 提供扩展方法以便在 <see cref="IApplicationBuilder"/> 中使用速率限制中间件。
    /// </summary>
    public static class RateLimiterApplicationBuilderExtensions
    {
        /// <summary>
        /// 将 <see cref="RateLimiterDataAnnotationsMiddleware"/> 添加到应用程序的请求管道中。
        /// </summary>
        /// <param name="app">应用程序构建器。</param>
        /// <returns>更新后的应用程序构建器。</returns>
        public static IApplicationBuilder UseRateLimiterDataAnnotations(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimiterDataAnnotationsMiddleware>();
        }
    }
}
