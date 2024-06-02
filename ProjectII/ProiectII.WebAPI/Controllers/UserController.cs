﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectII.BusinessModels.Models;
using ProjectII.DataAccess.Sqlite;

namespace ProjectII.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly CFRContext CFRcontext;

        public UserController(CFRContext CFRcontext)
        {
            this.CFRcontext = CFRcontext ?? throw new ArgumentNullException(nameof(CFRcontext));
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(CFRcontext.Users.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Train>> GetUser(int id)
        {
            User user = CFRcontext.Users.Find(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            CFRcontext.Users.Add(user);
            await CFRcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(AddUser), user, new
            {
                id = user.Id,
                email = user.Email,
                username = user.UserName,
                password = user.Password
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginUser)
        {
            // Access the username and password from the request
            string username = loginUser.UserName;
            string password = loginUser.Password;

            // Find the user by their username
            User user = await CFRcontext.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null || user.Password != password)
            {
                // Return Unauthorized if the user is not found or the password is incorrect
                return Unauthorized();
            }

            // Return the user details (excluding sensitive information like password)
            return Ok(new
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPut("{request.id}")]
        public async Task<ActionResult<List<Train>>> UpdateUser(User user)
        {
            User userUpdate = CFRcontext.Users.Find(user.Id);

            if (userUpdate == null)
            {
                return NotFound();
            }

            userUpdate.UserName = user.UserName;
            userUpdate.Password = user.Password;
            userUpdate.Email = user.Email;

            await CFRcontext.SaveChangesAsync();

            return Ok(userUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Train>> Delete(int id)
        {
            foreach (var user in CFRcontext.Users)
            {
                if (user.Id == id)
                    CFRcontext.Users.Remove(user);
            }

            await CFRcontext.SaveChangesAsync();
            return Ok();
        }
    }
}
