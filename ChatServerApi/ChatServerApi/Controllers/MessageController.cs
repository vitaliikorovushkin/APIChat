using ChatServerApi.Data;
using ChatServerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ChatServerApiDbContext _context;

        public MessageController(ChatServerApiDbContext context)
        {
            _context = context;
        }

        // GET: api/message
        [HttpGet("Index/{id}")]
        public async Task<ChatViewModel> Index(string id)
        {
            ChatViewModel chatViewModel = new ChatViewModel();
            chatViewModel.Users = await _context.User.Where(u => u.IsActive == true)
                .ToListAsync<User>();
            //chatViewModel.Messages = await _context.Message.Where(m => (Convert.ToInt32(DateTime.Now.Hour) - Convert.ToInt32(m.CreatedAt.Hour)) > 24)
            //    .ToListAsync<ChatMessage>();
            chatViewModel.Messages = await _context.Message.Where(m => m.To == "All" || m.To == id).ToListAsync<ChatMessage>();
            return chatViewModel;
        }

        // POST: api/Message/Send
        [HttpPost("Send")]
        public async Task<ActionResult> Send(ChatMessage msg)
        {
            msg.CreatedAt = DateTime.Now;
            _context.Message.Add(msg);
            await _context.SaveChangesAsync();
            string id = msg.From == null? "All" : msg.From;
            return RedirectToAction("Index", id);

        }
    }
}
