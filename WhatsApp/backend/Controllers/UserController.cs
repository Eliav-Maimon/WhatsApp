using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace backend.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController:ControllerBase
    {
        private readonly IMongoDatabase _mongoDatabase;

        public UserController(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        [HttpGet("{id:int}")]
        public ActionResult<User> GetUserByPhone(int id)
        {
            // TODO: verify code

            Console.WriteLine(id);
            var user = _mongoDatabase.GetCollection<User>("Users").Find(u => u._id == id).FirstOrDefault();

            if(user != null)
            {
                return Ok(user);
            }
            
            return NotFound();
        }
        
        [HttpGet("{phoneNumber}")]
        public ActionResult<User> GetUserByPhone(string phoneNumber)
        {
            // TODO: verify code

            Console.WriteLine(phoneNumber);
            var user = _mongoDatabase.GetCollection<User>("Users").Find(u => u.Phone == phoneNumber).FirstOrDefault();

            if(user != null)
            {
                return Ok(user);
            }
            
            return NotFound();
        }
    }


}