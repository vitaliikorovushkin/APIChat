using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatServerApi.Data;
using ChatServerApi.Models;

namespace ChatServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ChatServerApiDbContext _context;

        public UsersController(ChatServerApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.User.Where(u => u.IsActive == true).ToListAsync<User>();
        }

        // GET: api/Users/Login
        [HttpPut("Login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var userFromDb = await _context.User.FirstOrDefaultAsync(u => u.Name == user.Name &&
                u.Password == user.Password);

            if (userFromDb == null)
            {
                return NotFound();
            }
            userFromDb.IsActive = true;
            _context.Update(userFromDb);
            await _context.SaveChangesAsync();
            return userFromDb;
        }

        // GET: api/Users/Logout
        [HttpPut("Logout/{id}")]
        public async Task<ActionResult<User>> Logout(int id)
        {
            var userFromDb = await _context.User.FindAsync(id);

            if (userFromDb == null)
            {
                return NotFound();
            }
            userFromDb.IsActive = false;
            _context.Update(userFromDb);
            await _context.SaveChangesAsync();
            return userFromDb;
        }

        // POST: api/Users/Register
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            var userFromDb = await _context.User.FirstOrDefaultAsync(u => u.Name == user.Name);
            if(userFromDb == null)
            {
                user.CreatedAt = DateTime.Now;
                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("GetUsers");
        }

    }
}
