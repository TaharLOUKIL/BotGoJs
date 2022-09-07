using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    public class LocationModel  
    {
        #region Properties
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        private string _titre;
        private DateTime _createdAt;
        private DateTime _modifiedAt;
        private string _longitude { get; set; }
        private string _lattitude { get; set; }
        private string _adresse { get; set; }
        private string _type;
        private IConfiguration _configuration;

        //public string Id
        //{
        //    get { return this._id; }
        //    set { this._id = value; }
        //}

        public string Titre
        {
            get { return this._titre; }
            set { this._titre = value; }
        }

        public string Longitude
        {
            get { return this._longitude; }
            set { this._longitude = value; }
        }

        public string Lattitude
        {
            get { return this._lattitude; }
            set { this._lattitude = value; }
        }

        public string Adresse
        {
            get { return this._adresse; }
            set { this._adresse = value; }
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
        #endregion
        #region Methods

        /// <summary>
        /// Constructeur
        /// </summary>
        public LocationModel()
        {
        }
        public LocationModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JsonResult LoadAll()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<LocationModel>("Localisation").AsQueryable();
            return new JsonResult(dblist);
        }
        public Boolean Save(LocationModel data)
        {
            try
            {
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                data._id = ObjectId.GenerateNewId().ToString();
                data.Type = "Location";
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<LocationModel>("Localisation").InsertOne(data);
                return true;
            }
            catch (Exception)
            {
                // Add log
                return false;
            }
        }
        public Boolean Update(LocationModel data)
        {
            try
            {
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                var filter = Builders<LocationModel>.Filter.Eq("_id", data._id);
                data.Type = "Location";
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<LocationModel>("Localisation").ReplaceOne(filter, data);
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
                var filter = Builders<LocationModel>.Filter.Eq("_id", id);
                dbClient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<LocationModel>("Localisation").DeleteOne(filter);
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
