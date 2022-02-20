using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatServerApi.Models;

namespace ChatServerApi.Data
{
    public class ChatServerApiDbContext : DbContext
    {
        public ChatServerApiDbContext (DbContextOptions<ChatServerApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatServerApi.Models.User> User { get; set; }
        public DbSet<ChatServerApi.Models.ChatMessage> Message { get; set; }
    }
}
