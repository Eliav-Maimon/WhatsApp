using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        // private readonly IConfiguration _configuration;
        // private IIdentityRepository<User> _identity;

        // public IdentityController(IConfiguration configuration, IIdentityRepository<User> identity)
        // {
        //     _configuration = configuration;
        //     _identity = identity;
        // }

        // [HttpPost("SignIn")]
        // public IActionResult SignIn([FromBody] User user)
        // {
        //     try
        //     {
        //         var userToken = _identity.GetToken(user, _configuration.GetSection("JwtSettings").Get<JwtSettings>());
        //         if (userToken != null)
        //         {
        //             return Ok(userToken);
        //         }

        //         return NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"An error occurred while loading users: {ex.Message}");
        //         return NotFound();
        //     }
        // }
    }
}