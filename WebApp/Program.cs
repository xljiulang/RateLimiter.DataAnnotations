using UnitRateLimiter;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRateLimiter(o => o.AddUnitRateLimiterPolicies());

            var app = builder.Build();
 
            app.UseRouting();
          
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseUnitRateLimiter();
            app.UseRateLimiter();
            app.MapControllers().AddUnitRateLimiterPolicyConventions();

            app.Run();
        }
    }
}
