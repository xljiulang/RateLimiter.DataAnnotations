using Json.Path;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace RateLimiting.DataAnnotations
{
    partial class RateLimiterUnit
    {
        /// <summary>
        /// 指定限流单元单位来源是请求体json的特性。
        /// </summary>
        public class FromBodyAttribute : RateLimiterUnit
        {
            /// <summary>
            /// 获取请求体Json中表示限流单元的JsonPath。
            /// </summary>
            public JsonPath UnitName { get; }

            /// <summary>
            /// 初始化 <see cref="FromBodyAttribute"/> 类的新实例。
            /// </summary>
            /// <param name="unitName">请求体Json中表示限流单元的JsonPath。例如$.userId</param>
            public FromBodyAttribute(string unitName)
            {
                var jsonPath = JsonPath.Parse(unitName);
                if (!jsonPath.IsSingular)
                {
                    throw new ArgumentException($"{unitName} is not a singular JsonPath.", nameof(unitName));
                }

                UnitName = jsonPath;
            }

            /// <inheritdoc></inheritdoc>/>
            public override async ValueTask<string?> GetUnitAsync(HttpContext context)
            {
                context.Request.EnableBuffering();
                var position = context.Request.Body.Position;

                try
                {
                    return await this.ReadUnitFromJsonAsync(context.Request.Body, context.RequestAborted);
                }
                catch (OperationCanceledException)
                {
                    throw;
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
            /// <param name="stream">请求体流。</param> 
            /// <param name="cancellationToken">取消令牌。</param>
            /// <returns>表示异步操作的任务。任务结果包含单元标识符，如果无法检索则为 null。</returns>
            private async ValueTask<string?> ReadUnitFromJsonAsync(Stream stream, CancellationToken cancellationToken)
            {
                var jsonNode = await JsonNode.ParseAsync(stream, default, default, cancellationToken);
                var pathResult = this.UnitName.Evaluate(jsonNode);

                if (pathResult.Matches.TryGetSingleValue(out var unitNode) &&
                    unitNode != null &&
                    unitNode.TryGetValue<JsonElement>(out var unitElement))
                {
                    switch (unitElement.ValueKind)
                    {
                        case JsonValueKind.String:
                            return unitElement.GetString();

                        case JsonValueKind.Number:
                            return unitElement.GetRawText();

                        case JsonValueKind.True:
                            return "true";

                        case JsonValueKind.False:
                            return "false";
                    }
                }

                return null;
            }
        }
    }
}
