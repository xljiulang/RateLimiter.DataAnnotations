namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRateLimiter(o => o.AddRateLimiterDataAnnotations());

            var app = builder.Build();
 
            app.UseRouting();
          
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseRateLimiterDataAnnotations();
            app.UseRateLimiter();
            app.MapControllers().AddRateLimiterDataAnnotations();

            app.Run();
        }
    }
}
