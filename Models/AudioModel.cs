using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace BotGoJs.Models
{
    public class AudioModel
    {
        #region Properties

        private string _id;
        private string _titre;
        private string _url;
        private DateTime _createdAt;
        private DateTime _modifiedAt;
        private string _type;
        private IConfiguration _configuration;

        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Titre
        {
            get { return this._titre; }
            set { this._titre = value; }
        }

        public string Url
        {
            get { return this._url; }
            set { this._url = value; }
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

        public AudioModel()
        {
        }

        public AudioModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JsonResult LoadAll()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").AsQueryable();
            return new JsonResult(dblist);
        }

        public Boolean Save(AudioModel data)
        {
            // faire comme textModel
        }

        public Boolean Update(AudioModel data)
        {
            // faire comme textModel
        }

        public Boolean Delete(string id)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

                var filter = Builders<AudioModel>.Filter.Eq("_id", id);

                dbClient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").DeleteOne(filter);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Methods
    }
}