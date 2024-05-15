using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    public class SeedController:ControllerBase
    {
        public SeedController()
        {
            
        }
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
             
        }
    }
}