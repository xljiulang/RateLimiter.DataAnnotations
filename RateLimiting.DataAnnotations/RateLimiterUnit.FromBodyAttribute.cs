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
            /// 限流单元单位来源是请求体json的特性。
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
