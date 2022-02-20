using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Models
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
        }
        public List<User> Users { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }
}
