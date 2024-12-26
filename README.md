## RateLimiting.DataAnnotations
在Asp.netCore中使用Attribute来描述限流。

### Nuget
[RateLimiting.DataAnnotations](https://www.nuget.org/packages/RateLimiting.DataAnnotations)

### 如何使用
#### 服务注册和使用中间件
```c#
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    // 注册限流的DataAnnotations
    builder.Services.AddRateLimiterDataAnnotations((context, cancellationToken) =>
    {
        var unit = context.HttpContext.Features.Get<IRateLimiterUnitFeature>()?.Unit;
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

#### 为Action指定特性
```c#
[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    [RateLimiterUnit.FromRoute(unitName: "id")]
    [RateLimiter.FixedWindow(permitLimit: 8, windowSeconds: 60)]
    public User Get(string id)
    {
        return new User { Id = id };
    }

    [HttpPost]
    [RateLimiterUnit.FromBody(unitName: "$.id")]
    [RateLimiterUnit.FromBody(unitName: "$.name")]
    [RateLimiterUnit.FromRemoteIPAddress()]
    [RateLimiter.SlidingWindow(permitLimit: 9, windowSeconds: 60, segmentsPerWindow: 10)]
    public User Post(User user)
    {  
        return user;
    }

    [HttpDelete("{id}")]
    [RateLimiterUnit.FromUser(unitName: ClaimTypes.NameIdentifier)]
    [RateLimiter.SlidingWindow(permitLimit: 10, windowSeconds: 60, segmentsPerWindow: 10)]
    public bool Delete(string id)
    {
        return true;
    }
}
```
