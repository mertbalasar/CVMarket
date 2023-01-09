using CVMarket.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Business.Models.EntitiesModels
{
    public class MarketModel
    {
        public string UserId { get; set; }
        public string CVId { get; set; }
        public int Rate { get; set; }
        public List<CommentModel> Comments { get; set; } = new List<CommentModel> { };
    }

    public class CommentModel
    {
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
