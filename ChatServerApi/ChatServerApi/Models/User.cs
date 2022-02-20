using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Models
{
    public class User : BaseModel
    {
        public User() 
        {
            this.PartitionKey = "User";
        }
        public int Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
