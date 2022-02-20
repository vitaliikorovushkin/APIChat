using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Models
{
    public class ChatMessage : BaseModel
    {
        public ChatMessage()
        {
            this.PartitionKey = "Message";
        }
        public int Id { get; set; } 
        public String From { get; set; }
        public String To { get; set; }
        public String MessageText { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
