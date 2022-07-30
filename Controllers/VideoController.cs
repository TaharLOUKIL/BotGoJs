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
    public class VideoController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public VideoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

            var filter = Builders<VideoModel>.Filter.Eq("_id", id);


            dbClient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").DeleteOne(filter);

            return get();
        }

        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").AsQueryable();
            return new JsonResult(dblist);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            try
            {
                VideoModel video = new VideoModel();
                var formCollection = await Request.ReadFormAsync();

                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/video";
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
                        video.url = "http://localhost:12195/Resources/video/" + fileName;
                    }
                }
                else
                {
                    video.url = formCollection["url"];
                }

                video.createdAt = DateTime.Now;
                video.modifiedAt = DateTime.Now;
                video.Titre = formCollection["Titre"];
                video._id = ObjectId.GenerateNewId().ToString();
                video.type = "Video";
                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                dbclient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").InsertOne(video);
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
                VideoModel video = new VideoModel();
                var formCollection = await Request.ReadFormAsync();

                if (formCollection["url"].ToString().Length == 0)
                {
                    var file = formCollection.Files.First();
                    var folderName = "Resources/video";
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
                        video.url = "http://localhost:12195/Resources/video/" + fileName;
                    }
                }
                else
                {
                    video.url = formCollection["url"];
                }

                MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
                video.type = "Video";
                video.createdAt = DateTime.Parse(formCollection["createdAt"]);
                video.modifiedAt = DateTime.Now;
                video.Titre = formCollection["Titre"];
                video._id = formCollection["ID"];
                var filter = Builders<VideoModel>.Filter.Eq("_id", video._id);
                dbclient.GetDatabase("Gojs").GetCollection<VideoModel>("Video").ReplaceOne(filter, video);
                return get();
            }
            catch (Exception ex)
            {
                return new JsonResult(StatusCode(500, $"Internal server error: {ex}"));
            }


        }

    }
}
