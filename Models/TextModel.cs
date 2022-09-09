using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace BotGoJs.Models
{
    /// <summary>
    /// Modèle qui servira à la persistance des textes
    /// </summary>
    public class TextModel
    {
        #region Properties

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        private string _message;
        private DateTime _createdAt;
        private DateTime _modifiedAt;
        private string _type;
        private IConfiguration _configuration;

        //public string Id
        //{
        //    get { return this._id; }
        //    set { this._id = value; }
        //}

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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructeur
        /// </summary>
        public TextModel()
        {
        }

        public TextModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JsonResult LoadAll()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<TextModel>("Text").AsQueryable();
            return new JsonResult(dblist);
        }

        public Boolean Save(TextModel data)
        {
            try
            {
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                data._id = ObjectId.GenerateNewId().ToString();
                data.Type = "Text";
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<TextModel>("Text").InsertOne(data);
                return true;
            }
            catch (Exception)
            {
                // Add log
                return false;
            }
        }

        public Boolean Update(TextModel data)
        {
            try
            {
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                var filter = Builders<TextModel>.Filter.Eq("_id", data._id);
                data.Type = "Text";
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<TextModel>("Text").ReplaceOne(filter, data);
                return true;
            }
            catch (Exception)
            {
                // Add log
                return false;
            }
        }

        public Boolean Delete(string id)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                var filter = Builders<TextModel>.Filter.Eq("_id", id);
                dbClient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<TextModel>("Text").DeleteOne(filter);
                return true;
            }
            catch (Exception)
            {
                // Ajout de log
                return false;
            }
        }

        #endregion Methods
    }
}