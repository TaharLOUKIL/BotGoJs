using BotGoJs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    { 
     public readonly IConfiguration _configuration;

    public AudioController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpDelete("{id}")]
    public JsonResult Delete(string id)
    {
        MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

        var filter = Builders<AudioModel>.Filter.Eq("_id", id);


        dbClient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").DeleteOne(filter);

        return get();
    }

    [HttpGet]
    public JsonResult get()
    {
        MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
        var dblist = dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").AsQueryable();
        return new JsonResult(dblist);
    }

    [HttpPost, DisableRequestSizeLimit]
    public async Task<JsonResult> upload()
    {
        try
        {
                AudioModel audio = new AudioModel();
            var formCollection = await Request.ReadFormAsync();

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
                    audio.url = "http://localhost:12195/Resources/audio/" + fileName;
                }
            }
            else
            {
                audio.url = formCollection["url"];
            }

            audio.createdAt = DateTime.Now;
            audio.modifiedAt = DateTime.Now;
            audio.Titre = formCollection["Titre"];
            audio.type = "Audio";
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            audio._id = ObjectId.GenerateNewId().ToString();
            dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").InsertOne(audio);
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
                AudioModel audio = new AudioModel();
            var formCollection = await Request.ReadFormAsync();

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
                    audio.url = "http://localhost:12195/Resources/audio/" + fileName;
                }
            }
            else
            {
                audio.url = formCollection["url"];
            }

            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));


            audio.createdAt = DateTime.Parse(formCollection["createdAt"]);
            audio.modifiedAt = DateTime.Now;
            audio.Titre = formCollection["Titre"];
            audio._id = formCollection["ID"];
                audio.type = "Audio";
                var filter = Builders<AudioModel>.Filter.Eq("_id", audio._id);
            dbclient.GetDatabase("Gojs").GetCollection<AudioModel>("Audio").ReplaceOne(filter, audio);
            return get();
        }
        catch (Exception ex)
        {
            return new JsonResult(StatusCode(500, $"Internal server error: {ex}"));
        }


    }

}
}