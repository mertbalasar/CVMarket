using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Entities.Base
{
    public class CollectionBase : ICollectionBase
    {
        [BsonRepresentation(BsonType.ObjectId), BsonId]
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
