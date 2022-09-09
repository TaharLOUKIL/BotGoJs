using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BotGoJs.Models
{
    /// <summary>
    /// Modèle qui servira à la persistance des Bots créés depuis le front
    /// </summary>
    public class BotModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        //Nom du bot
        public string name { get; set; }
    }
}