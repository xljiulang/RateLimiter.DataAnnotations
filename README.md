## RateLimiter.DataAnnotations
能让你在Asp.netCore中使用Attribute来配置策略一个项目。

## 如何使用
### 服务注册和使用中间件
```c#
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    // 注册限流的DataAnnotations
    builder.Services.AddRateLimiterDataAnnotations();
    // 配置限流触发后的响应
    builder.Services.AddRateLimiter(x => x.OnRejected = (context, cancellationToken) =>
    {
        var unit = context.HttpContext.Features.Get<IUnitFeature>()?.Unit;
        context.HttpContext.Response.Headers.TryAdd("X-RateLimit-Unit", unit);
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        return ValueTask.CompletedTask;
    });

    var app = builder.Build();

    app.UseRouting();

    // app.UseAuthentication();
    // app.UseAuthorization();

    // UseRateLimiterDataAnnotations必须在UseRateLimiter之前，否则RateLimiterUnit不生效
    app.UseRateLimiterDataAnnotations();
    app.UseRateLimiter();

    // 为Controller的endpoint添加DataAnnotations约定
    app.MapControllers().AddRateLimiterDataAnnotations();

    app.Run();
}
```

### 为Action指定特性
```c#
[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    [RateLimiterUnit.FromRoute(unitName: "id")] // RateLimiterUnit是可选的
    [RateLimiterPolicy.FixedWindowLimiter(permitLimit: 8, windowSeconds: 60)]
    public User Get(string id)
    {
        return new User { Id = id };
    }

    [HttpPost]
    [RateLimiterUnit.FromBody(unitName: "id")] / RateLimiterUnit是可选的
    [RateLimiterPolicy.SlidingWindowLimiter(permitLimit: 9, windowSeconds: 60, segmentsPerWindow: 10)]
    public User Post(User user)
    {
        return user;
    }

    [HttpDelete("{id}")]
    [RateLimiterUnit.FromUser(unitName: ClaimTypes.NameIdentifier)] / RateLimiterUnit是可选的
    [RateLimiterPolicy.SlidingWindowLimiter(permitLimit: 10, windowSeconds: 60, segmentsPerWindow: 10)]
    public bool Delete(string id)
    {
        return true;
    }
}
```
