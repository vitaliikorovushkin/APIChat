using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerApi.Models
{
    public class BaseModel : TableEntity
    {
        public BaseModel()
        {
            this.RowKey = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + new Random()
                .Next(1000000)).ToString();
        }
    }
}
