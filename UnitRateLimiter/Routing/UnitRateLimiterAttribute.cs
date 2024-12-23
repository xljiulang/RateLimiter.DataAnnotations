using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    /// <summary>
    /// 指定限流单元来源的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class UnitRateLimiterAttribute : Attribute, IUnitRateLimiterMetadata
    {
        /// <summary>
        /// 获取限流单元的来源。
        /// </summary>
        public UnitSource UnitSource { get; }

        /// <summary>
        /// 获取单元的名称。
        /// </summary>
        public string UnitName { get; } = string.Empty;

        /// <summary>
        /// 使用指定的单元来源初始化 <see cref="UnitRateLimiterAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="unitSource">限流单元的来源。</param>
        public UnitRateLimiterAttribute(UnitSource unitSource)
        {
            this.UnitSource = unitSource;
        }

        /// <summary>
        /// 使用指定的单元来源和单元名称初始化 <see cref="UnitRateLimiterAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="unitSource">限流单元的来源。</param>
        /// <param name="unitName">单元的名称。</param>
        public UnitRateLimiterAttribute(UnitSource unitSource, string unitName)
        {
            this.UnitSource = unitSource;
            this.UnitName = unitName;
        }

        /// <summary>
        /// 异步从指定的 HTTP 上下文获取单元。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <returns>表示异步操作的任务。任务结果包含单元。</returns>
        async ValueTask<string?> IUnitRateLimiterMetadata.GetUnitAsync(HttpContext context)
        {
            switch (UnitSource)
            {
                case UnitSource.User:
                    return context.User.FindFirstValue(UnitName);

                case UnitSource.Route:
                    return context.GetRouteValue(UnitName)?.ToString();

                case UnitSource.Query:
                    {
                        return context.Request.Query.TryGetValue(UnitName, out var unit) ? (string?)unit : null;
                    }

                case UnitSource.Header:
                    {
                        return context.Request.Headers.TryGetValue(UnitName, out var unit) ? (string?)unit : null;
                    }

                case UnitSource.Form:
                    {
                        return context.Request.Form.TryGetValue(UnitName, out var unit) ? (string?)unit : null;
                    }

                case UnitSource.Body:
                    {
                        context.Request.EnableBuffering();
                        var position = context.Request.Body.Position;

                        try
                        {
                            return await ReadUnitFromJsonAsync(context.Request.Body, UnitName, context.RequestAborted);
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                        finally
                        {
                            context.Request.Body.Position = position;
                        }
                    }

                case UnitSource.RemoteIPAddress:
                    return context.Connection.RemoteIpAddress?.ToString();

                default:
                    return null;
            }
        }

        /// <summary>
        /// 异步从 JSON 请求体中读取单元。
        /// </summary>
        /// <param name="body">请求体流。</param>
        /// <param name="unitName">单元的名称。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>表示异步操作的任务。任务结果包含单元。</returns>
        private static async ValueTask<string?> ReadUnitFromJsonAsync(Stream body, string unitName, CancellationToken cancellationToken)
        {
            using var document = await JsonDocument.ParseAsync(body, default, cancellationToken);
            if (document.RootElement.TryGetProperty(unitName, out var unitElement))
            {
                switch (unitElement.ValueKind)
                {
                    case JsonValueKind.String:
                        return unitElement.GetString();

                    case JsonValueKind.True:
                        return "true";

                    case JsonValueKind.False:
                        return "false";

                    case JsonValueKind.Number:
                        return unitElement.GetRawText();
                }
            }
            return null;
        }
    }
}
