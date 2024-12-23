using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UnitLimiter.Metadatas
{
    public interface ILimiterUnitMetadataProvider
    {
        ValueTask<string?> GetUnitAsync(HttpContext context);
    }
}
