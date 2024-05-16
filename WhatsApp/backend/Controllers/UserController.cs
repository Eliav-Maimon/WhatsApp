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

        [HttpGet("{phoneNumber:long}")] //user/123456789
        public ActionResult<User> GetUserByPhone(long phoneNumber)
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

        [HttpGet("{id}")] //user/ljfsd58fdf
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

        [HttpPost("{currentUserId}/{contactUserId}")] 
        public ActionResult<User> AddContact(string currentUserId, string contactUserId)
        {
            
            Console.WriteLine("currentUserId "+ currentUserId +" contactUserId "+ contactUserId);
            var users = _mongoDatabase.GetCollection<User>("Users");

            var currentUser = users.Find(u => u.Id == currentUserId).FirstOrDefault();
            System.Console.WriteLine("currentUser "+ currentUser);
            var contactUser = users.Find(u => u.Id == contactUserId).FirstOrDefault();
            System.Console.WriteLine("contactUser "+ contactUser);

            if(currentUser != null && contactUser != null)
            {
                currentUser.Contacts.Add(contactUser.Id);
                
                var filter = Builders<User>.Filter.Eq(u => u.Id, currentUserId);
                Console.WriteLine(filter);
                var update = Builders<User>.Update.Set(u => u.Contacts, currentUser.Contacts);
                Console.WriteLine(update);
                users.UpdateOne(filter, update);
                
                return Ok(currentUser);
            }
            
            return NotFound();
        }

        [HttpDelete("{id}")] //user/ljfsd58fdf
        public ActionResult<User> RemoveContact(string currentUserId, string contactUserId)
        {
            Console.WriteLine("currentUserId "+ currentUserId +" contactUserId "+ contactUserId);
            var currentUser = _mongoDatabase.GetCollection<User>("Users").Find(u => u.Id == currentUserId).FirstOrDefault();
            var contactUser = _mongoDatabase.GetCollection<User>("Users").Find(u => u.Id == contactUserId).FirstOrDefault();

           if(currentUser != null && contactUser != null)
            {
                // currentUser.Contacts.Remove(contactUser);
               
                return Ok(currentUser);
            }
            
            return NotFound();
        }
        
    }


}