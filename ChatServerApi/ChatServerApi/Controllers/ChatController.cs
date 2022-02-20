using ChatServerApi.Models;
using ChatServerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ITableService service;

        public ChatController(ITableService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return await service.GetTableItems<User>("User", null);
        }

        //[HttpGet]
        //public async Task<List<ChatMessage>> GetMessages()
        //{
        //    return await service.GetTableItems<ChatMessage>("Message", null);
        //}

        // POST: api/Users/Register
        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync(User user)
        {
            user.CreatedAt = DateTime.Now;
            await service.AddItem<User>(user);
            return RedirectToAction("GetUsers");
        }

        // GET: api/Users/Login
        [HttpPut("Login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var userFromDb = await service.GetTableItem<User>(user.Name, user.Password);
              
            if (userFromDb == null)
            {
                return NotFound();
            }
            userFromDb.IsActive = true;
            await service.UpdateTableItem<User>(userFromDb);
            return userFromDb;
        }

        // GET: api/message
        [HttpGet("Index/{id}")]
        public async Task<ChatViewModel> Index(string id)
        {
            ChatViewModel chatViewModel = new ChatViewModel();
            IEnumerable<User> listUsers = await GetUsers();
            //IEnumerable<ChatMessage> listMsg = await GetMessages();
            //chatViewModel.Users = await _context.User.Where(u => u.IsActive == true)
            //.ToListAsync<User>();
            chatViewModel.Users = listUsers.Where(u => u.IsActive == true).ToList<User>();
            //chatViewModel.Messages = await _context.Message.Where(m => (Convert.ToInt32(DateTime.Now.Hour) - Convert.ToInt32(m.CreatedAt.Hour)) > 24)
            //    .ToListAsync<ChatMessage>();
            //chatViewModel.Messages = await _context.Message.Where(m => m.To == "All" || m.To == id).ToListAsync<ChatMessage>();
            chatViewModel.Messages = null;
            return chatViewModel;
        }


        // GET: api/Users/Logout
        [HttpPut("Logout/{id}")]
        public async Task<ActionResult<User>> Logout(User user)
        {
            var userFromDb = await service.GetTableItem<User>(user.Name, user.Password);

            if (userFromDb == null)
            {
                return NotFound();
            }
            userFromDb.IsActive = false;
            await service.UpdateTableItem<User>(userFromDb);
            return userFromDb;
        }
    }
}
