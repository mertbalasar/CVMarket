using CVMarket.Entities.Attributes;
using CVMarket.Entities.Base;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Entities.Models
{
    [CollectionName("market")]
    public class Market : CollectionBase
    {
        public ObjectId UserId { get; set; }
        public ObjectId CVId { get; set; }
        public int Rate { get; set; } = 0;
        public int StarCount { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        public List<Comment> Comments { get; set; } = new List<Comment> { };
    }

    public class Comment
    {
        public ObjectId UserId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
