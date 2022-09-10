using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Linq;

namespace BotGoJs.Models
{
    /// <summary>
    /// Modèle qui servira à la persistance des Bots créés depuis le front
    /// </summary>
    public class BotModel
    {
        #region Properties

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        private string _name;
        private IConfiguration _configuration;

        //public string Id
        //{
        //    get { return this._id; }
        //    set { this._id = value; }
        //}

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Constructeur
        /// </summary>
        public BotModel()
        {
        }

        public BotModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JsonResult LoadAll()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<BotModel>("Bots").AsQueryable();
            return new JsonResult(dblist);
        }

        public Boolean Save(BotModel data)
        {
            try
            {
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                data._id = ObjectId.GenerateNewId().ToString();
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<BotModel>("Bots").InsertOne(data);
                return true;
            }
            catch (Exception)
            {
                // Add log
                return false;
            }
        }

        public Boolean Update(BotModel data)
        {
            try
            {
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                var filter = Builders<BotModel>.Filter.Eq("_id", data._id);
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<BotModel>("Bots").ReplaceOne(filter, data);
                return true;
            }
            catch (Exception)
            {
                // Add log
                return false;
            }
        }

        #endregion Methods
    }
}