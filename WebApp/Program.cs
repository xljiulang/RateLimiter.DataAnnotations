namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRateLimiterDataAnnotations();
            builder.Services.AddRateLimiter(x => x.OnRejected = (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return ValueTask.CompletedTask;
            });

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
