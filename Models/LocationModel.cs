using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BotGoJs.Models
{
    public class LocationModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime modifiedAt { get; set; }

        public string Longitude { get; set; }

        public string Lattitude { get; set; }

        public string Adresse { get; set; }

        public string Titre { get; set; }
        public string type { get; set; }
    }
}