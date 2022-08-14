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
    public class ImageController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

            var filter = Builders<ImageModel>.Filter.Eq("_id", id);

            dbClient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").DeleteOne(filter);

            return get();
        }

        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").AsQueryable();
            return new JsonResult(dblist);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            try
            {
                ImageModel image = new ImageModel();
                var formCollection = await Request.ReadFormAsync();

                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/image";
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
                        image.url = "http://localhost:12195/Resources/image/" + fileName;
                    }
                }
                else
                {
                    image.url = formCollection["url"];
                }

                image.createdAt = DateTime.Now;
                image.modifiedAt = DateTime.Now;
                image.Titre = formCollection["Titre"];
                image.type = "Image";
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                image._id = ObjectId.GenerateNewId().ToString();
                dbclient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").InsertOne(image);
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
                ImageModel image = new ImageModel();
                var formCollection = await Request.ReadFormAsync();

                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/image";
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
                        image.url = "http://localhost:12195/Resources/image/" + fileName;
                    }
                }
                else
                {
                    image.url = formCollection["url"];
                }

                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

                image.createdAt = DateTime.Parse(formCollection["createdAt"]);
                image.modifiedAt = DateTime.Now;
                image.type = "Image";
                image.Titre = formCollection["Titre"];
                image._id = formCollection["ID"];
                var filter = Builders<ImageModel>.Filter.Eq("_id", image._id);
                dbclient.GetDatabase("Gojs").GetCollection<ImageModel>("Image").ReplaceOne(filter, image);
                return get();
            }
            catch (Exception ex)
            {
                return new JsonResult(StatusCode(500, $"Internal server error: {ex}"));
            }
        }
    }
}