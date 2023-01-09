using CVMarket.Entities.Attributes;
using CVMarket.Entities.Base;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVMarket.Entities.Models
{
    [CollectionName("file")]
    public class Files : CollectionBase
    {
        public string FilePath { get; set; }
        public ObjectId UserId { get; set; }
    }
}
