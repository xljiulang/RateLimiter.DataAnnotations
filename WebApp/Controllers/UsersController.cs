using Microsoft.AspNetCore.Mvc;
using RateLimiter.DataAnnotations;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        [RateLimiterUnit.FromRoute(unitName: "id")]
        [RateLimiterPolicy.SlidingWindowLimiter(permitLimit: 8, windowSeconds: 60, segmentsPerWindow: 10)]
        public User Get(string id)
        {
            return new User { Id = id };
        }

        [HttpPost]
        [RateLimiterUnit.FromBody(unitName: "id")]
        [RateLimiterPolicy.SlidingWindowLimiter(permitLimit: 9, windowSeconds: 60, segmentsPerWindow: 10)]
        public User Post(User user)
        {
            return user;
        }

        [HttpDelete("{id}")]
        [RateLimiterUnit.FromUser(unitName: ClaimTypes.NameIdentifier)]
        [RateLimiterPolicy.SlidingWindowLimiter(permitLimit: 10, windowSeconds: 60, segmentsPerWindow: 10)]
        public bool Delete(string id)
        {
            return true;
        }
    }
}
