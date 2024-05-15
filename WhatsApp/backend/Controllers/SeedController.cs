using System.Text.Json;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("seed")]
    public class SeedController : ControllerBase
    {
        public string FilePath = "data.json";
        public SeedController()
        {

        }
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
           string jsonData = System.IO.File.ReadAllText(this.FilePath);
           System.Console.WriteLine("here: json data : " + jsonData);
           var data =  JsonSerializer.Deserialize<List<User>>(jsonData);

           return data;
        }
    }
}