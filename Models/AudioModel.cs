using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace BotGoJs.Models
{
    public class AudioModel
    {
        #region Properties
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        private string _titre;
        private string _url;
        private DateTime _createdAt;
        private DateTime _modifiedAt;
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
        #endregion
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
            var dblist = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<AudioModel>("Audio").AsQueryable();
            return new JsonResult(dblist);
        }

        public Boolean Save(IFormCollection formCollection )
        {
            try
            {
                AudioModel audio = new AudioModel();
                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/audio";
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        audio.Url = _configuration["Variable:FilesRoute"] +"audio/" + fileName;
                    }
                }
                else
                {
                    audio.Url = formCollection["url"];
                }

                audio.CreatedAt = DateTime.Now;
                audio.ModifiedAt = DateTime.Now;
                audio.Titre = formCollection["Titre"];
                audio.Type = "Audio";
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                audio._id = ObjectId.GenerateNewId().ToString();
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<AudioModel>("Audio").InsertOne(audio);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean Update(IFormCollection formCollection)
        {
            try
            {
                AudioModel audio = new AudioModel();
                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/audio";
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        audio.Url = _configuration["Variable:FilesRoute"] +"audio/" + fileName;
                    }
                }
                else
                {
                    audio.Url = formCollection["url"];
                }

                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));


                audio.CreatedAt = DateTime.Parse(formCollection["createdAt"]);
                audio.ModifiedAt = DateTime.Now;
                audio.Titre = formCollection["Titre"];
                audio._id = formCollection["ID"];
                audio.Type = "Audio";
                var filter = Builders<AudioModel>.Filter.Eq("_id", audio._id);
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<AudioModel>("Audio").ReplaceOne(filter, audio);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public Boolean Delete(string id)
        {
            try
            {
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                var filter = Builders<AudioModel>.Filter.Eq("_id", id);
                dbClient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<AudioModel>("Audio").DeleteOne(filter);
                return true;
            }
            catch (Exception)
            {
                // Ajout de log
                return false;
            }
        }
        #endregion



    }
}