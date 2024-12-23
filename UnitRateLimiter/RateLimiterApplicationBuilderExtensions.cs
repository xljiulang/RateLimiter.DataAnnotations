using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UnitRateLimiter.Middlewares;

namespace UnitRateLimiter
{
    /// <summary>
    /// 提供扩展方法以便在 <see cref="IApplicationBuilder"/> 中使用速率限制中间件。
    /// </summary>
    public static class RateLimiterApplicationBuilderExtensions
    {
        /// <summary>
        /// 将 <see cref="UnitRateLimiterMiddleware"/> 添加到应用程序的请求管道中。
        /// </summary>
        /// <param name="app">应用程序构建器。</param>
        /// <returns>更新后的应用程序构建器。</returns>
        public static IApplicationBuilder UseUnitRateLimiter(this IApplicationBuilder app)
        {
            var middleware = ActivatorUtilities.CreateInstance<UnitRateLimiterMiddleware>(app.ApplicationServices);
            return app.Use(next => context => middleware.InvokeAsync(context, next));
        }
    }
}
