using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    public class TextModel 
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string message { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime modifiedAt { get; set; }

        public string type { get; set; }

    }
}
