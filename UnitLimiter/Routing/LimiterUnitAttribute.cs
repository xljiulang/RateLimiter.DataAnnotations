using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnitLimiter.Metadatas;

namespace UnitLimiter.Routing
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class LimiterUnitAttribute : Attribute, ILimiterUnitMetadataProvider
    {
        public UnitSource UnitSource { get; }

        public string UnitName { get; }

        public LimiterUnitAttribute(UnitSource unitSource, string unitName)
        {
            this.UnitSource = unitSource;
            this.UnitName = unitName;
        }

        async ValueTask<string?> ILimiterUnitMetadataProvider.GetUnitAsync(HttpContext context)
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

                default:
                    throw new NotSupportedException(UnitSource.ToString());
            }
        }

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
