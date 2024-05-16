using System.Text.Json;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace backend.Controllers
{
    [ApiController]
    [Route("seed")]
    public class SeedController : ControllerBase
    {
        public string FilePath = "data.json";
        private readonly IMongoCollection<User> _userCollection;
        public SeedController(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users");
        }
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            string jsonData = System.IO.File.ReadAllText(this.FilePath);
            Console.WriteLine("here: json data : " + jsonData);
            var data = JsonSerializer.Deserialize<List<User>>(jsonData);
            
            _userCollection.DeleteMany(Builders<User>.Filter.Empty);
            _userCollection.InsertMany(data);

            return data;
        }
    }
}