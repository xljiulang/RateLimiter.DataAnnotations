using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UnitRateLimiter.Routing;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        [SlidingWindowLimiterPolicy(permitLimit: 10, windowSeconds: 60, segmentsPerWindow: 10)]
        [UnitRateLimiter(UnitSource.Route, unitName: "id")]
        public User Get(string id)
        {
            return new User { Id = id };
        }

        [HttpPost]
        [SlidingWindowLimiterPolicy(permitLimit: 10, windowSeconds: 60, segmentsPerWindow: 10)]
        [UnitRateLimiter(UnitSource.Body, unitName: "id")]
        public User Post(User user)
        {
            return user;
        }

        [HttpDelete("{id}")]
        [SlidingWindowLimiterPolicy(permitLimit: 10, windowSeconds: 60, segmentsPerWindow: 10)]
        [UnitRateLimiter(UnitSource.User, unitName: ClaimTypes.NameIdentifier)]
        public bool Delete(string id)
        {
            return true;
        }
    }
}
