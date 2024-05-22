using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WhatsApp.Repository;

namespace backend.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> _userRepository;

        public UserController(IUserRepository<User> mongoDatabase)
        {
            _userRepository = mongoDatabase;
        }

        [HttpGet("users")] //user/
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return users == null ? NotFound() : Ok(users);
        }

        [HttpGet("phone{phoneNumber}")] //user/123456789
        public async Task<ActionResult<User>> GetUserByPhone(string phoneNumber)
        {
            // TODO: verify code

            var user = await _userRepository.GetUserByPhoneAsync(phoneNumber);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("{id}")] //user/ljfsd58fdf
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpPost("{currentUserId}/{contactUserId}")]
        public async Task<ActionResult<User>> AddContact(string currentUserId, string contactUserId)
        {
            var user = await _userRepository.AddContactAsync(currentUserId, contactUserId);
            return user == null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpDelete("{currentUserId}/{contactUserId}")]
        public async Task<ActionResult<User>> RemoveContact(string currentUserId, string contactUserId)
        {
            var user = await _userRepository.RemoveContactAsync(currentUserId, contactUserId);
            return _userRepository == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> AddUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var newUser = await _userRepository.AddUserAsync(user);
                return newUser == null ? NotFound() : Ok(newUser);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<User>> RemoveUser([FromBody] string id)
        {
            var isDeleted = await _userRepository.RemoveUserAsync(id);
            if (isDeleted)
            {
                return Ok("User deleted successfully");
            }

            return NotFound();
        }
    }
}