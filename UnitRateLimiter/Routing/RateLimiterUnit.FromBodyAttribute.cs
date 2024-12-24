using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnitRateLimiter.Metadatas;

namespace UnitRateLimiter.Routing
{
    public static partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是请求体json的特性。
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
        public sealed class FromBodyAttribute : Attribute, IUnitRateLimiterMetadata
        {
            /// <summary>
            /// 获取单元的名称。
            /// </summary>
            public string UnitName { get; }

            public FromBodyAttribute(string unitName)
            {
                UnitName = unitName;
            }

            public async ValueTask<string?> GetUnitAsync(HttpContext context)
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
}
