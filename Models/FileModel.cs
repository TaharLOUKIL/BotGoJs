using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BotGoJs.Models
{
    public class FileModel
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

        /// <summary>
        /// Constructeur
        /// </summary>
        public FileModel()
        {
        }
        public FileModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JsonResult LoadAll()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<FileModel>("Fichier").AsQueryable();
            return new JsonResult(dblist);
        }
        public Boolean SaveAsync(IFormCollection formCollection)
        {
            try
            {
                FileModel fichier = new FileModel();

                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/file";
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
                        fichier.Url = _configuration["Variable:FilesRoute"] + "file/" + fileName;
                    }
                }
                else
                {
                    fichier.Url = formCollection["url"];
                }

                fichier.CreatedAt = DateTime.Now;
                fichier.ModifiedAt = DateTime.Now;
                fichier.Titre = formCollection["Titre"];
                fichier.Type = "File";
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                fichier._id = ObjectId.GenerateNewId().ToString();
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<FileModel>("Fichier").InsertOne(fichier);
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
                FileModel fichier = new FileModel();
                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/file";
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
                        fichier.Url = _configuration["Variable:FilesRoute"]+"file/" + fileName;
                    }
                }
                else
                {
                    fichier.Url = formCollection["url"];
                }

                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));


                fichier.CreatedAt = DateTime.Parse(formCollection["createdAt"]);
                fichier.ModifiedAt = DateTime.Now;
                fichier.Titre = formCollection["Titre"];
                fichier._id = formCollection["ID"];
                fichier.Type = "File";
                var filter = Builders<FileModel>.Filter.Eq("_id", fichier._id);
                dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<FileModel>("Fichier").ReplaceOne(filter, fichier);
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

                var filter = Builders<FileModel>.Filter.Eq("_id", id);


                dbClient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<FileModel>("Fichier").DeleteOne(filter);
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
