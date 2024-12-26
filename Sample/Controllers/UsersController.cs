﻿using Microsoft.AspNetCore.Mvc;
using RateLimiting.DataAnnotations;
using System.Security.Claims;

namespace Sample.Controllers
{
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
}
