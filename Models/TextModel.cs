using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    /// <summary>
    /// Modèle qui servira à la persistance des textes
    /// </summary>
    public class TextModel 
    {
        [BsonRepresentation(BsonType.ObjectId)]

        private string _id;
        private string _message;
        private DateTime _createdAt;
        private DateTime _modifiedAt;
        private string _type;

        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }

        public DateTime CreatedAt
        {
            get { return this._createdAt; }
            set { this._createdAt = value; }
        }

        public DateTime ModifiedAt
        {
            get { return this._modifiedAt; }
            set { this._modifiedAt = value; }
        }

        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

    }
}
