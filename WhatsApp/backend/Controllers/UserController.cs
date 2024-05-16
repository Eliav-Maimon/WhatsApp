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

        [HttpGet("{phoneNumber:int}")]
        public ActionResult<User> GetUserByPhone(int phoneNumber)
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

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(string id)
        {
            // TODO: verify code

            Console.WriteLine(id);
            var user = _mongoDatabase.GetCollection<User>("Users").Find(u => u.Id == id).FirstOrDefault();

            if(user != null)
            {
                return Ok(user);
            }
            
            return NotFound();
        }
        
    }


}