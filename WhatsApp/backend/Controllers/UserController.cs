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

        [HttpGet()] //user/
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var usersCollection = _mongoDatabase.GetCollection<User>("Users");

            var users = usersCollection.Find(_ => true).ToList();

            if(users != null)
            {
                return Ok(users);
            }
            
            return NotFound();
        }

        [HttpGet("{phoneNumber:long}")] //user/123456789
        public ActionResult<User> GetUserByPhone(long phoneNumber)
        {
            // TODO: verify code

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
            var users = _mongoDatabase.GetCollection<User>("Users");
            //  List<User> result = await users;
            var currentUser = users.Find(u => u.Id == currentUserId).FirstOrDefault();
            var contactUser = users.Find(u => u.Id == contactUserId).FirstOrDefault();

            if(currentUser != null && contactUser != null)
            {
                currentUser.Contacts.Add(contactUser.Id);
                
                var filter = Builders<User>.Filter.Eq(u => u.Id, currentUserId);
                var update = Builders<User>.Update.Set(u => u.Contacts, currentUser.Contacts);

                users.UpdateOne(filter, update);
                
                return Ok(currentUser);
            }
            
            return NotFound();
        }

        [HttpDelete("{currentUserId}/{contactUserId}")] 
        public ActionResult<User> RemoveContact(string currentUserId, string contactUserId)
        {
           var users = _mongoDatabase.GetCollection<User>("Users");

            var currentUser = users.Find(u => u.Id == currentUserId).FirstOrDefault();
            var contactUser = users.Find(u => u.Id == contactUserId).FirstOrDefault();

            if(currentUser != null && contactUser != null)
            {
                currentUser.Contacts.Remove(contactUser.Id);
                
                var filter = Builders<User>.Filter.Eq(u => u.Id, currentUserId);
                var update = Builders<User>.Update.Set(u => u.Contacts, currentUser.Contacts);
                
                users.UpdateOne(filter, update);
                
                return Ok(currentUser);
            }
            
            return NotFound();
        }
        
    }


}