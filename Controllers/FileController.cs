using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

            var filter = Builders<FileModel>.Filter.Eq("_id", id);

            dbClient.GetDatabase("Gojs").GetCollection<FileModel>("Fichier").DeleteOne(filter);

            return get();
        }

        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<FileModel>("Fichier").AsQueryable();
            return new JsonResult(dblist);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            try
            {
                FileModel fichier = new FileModel();
                var formCollection = await Request.ReadFormAsync();

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
                        fichier.url = "http://localhost:12195/Resources/file/" + fileName;
                    }
                }
                else
                {
                    fichier.url = formCollection["url"];
                }

                fichier.createdAt = DateTime.Now;
                fichier.modifiedAt = DateTime.Now;
                fichier.Titre = formCollection["Titre"];
                fichier.type = "File";
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                fichier._id = ObjectId.GenerateNewId().ToString();
                dbclient.GetDatabase("Gojs").GetCollection<FileModel>("Fichier").InsertOne(fichier);
                return get();
            }
            catch (Exception ex)
            {
                return new JsonResult(StatusCode(500, $"Internal server error: {ex}"));
            }
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<JsonResult> update()
        {
            try
            {
                FileModel fichier = new FileModel();
                var formCollection = await Request.ReadFormAsync();

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
                        fichier.url = "http://localhost:12195/Resources/file" + fileName;
                    }
                }
                else
                {
                    fichier.url = formCollection["url"];
                }

                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

                fichier.createdAt = DateTime.Parse(formCollection["createdAt"]);
                fichier.modifiedAt = DateTime.Now;
                fichier.Titre = formCollection["Titre"];
                fichier._id = formCollection["ID"];
                fichier.type = "File";
                var filter = Builders<FileModel>.Filter.Eq("_id", fichier._id);
                dbclient.GetDatabase("Gojs").GetCollection<FileModel>("Fichier").ReplaceOne(filter, fichier);
                return get();
            }
            catch (Exception ex)
            {
                return new JsonResult(StatusCode(500, $"Internal server error: {ex}"));
            }
        }
    }
}