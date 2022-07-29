using BotGoJs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TexteController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public TexteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").AsQueryable();
            return new JsonResult(dblist);
        }

        [HttpPost]
        public JsonResult Post(TextModel data)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            data._id = ObjectId.GenerateNewId().ToString();
            data.type = "Text";

            dbclient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").InsertOne(data);
            return get();
        }

        [HttpPut]
        public JsonResult Put(TextModel data)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var filter = Builders<TextModel>.Filter.Eq("_id", data._id);
            data.type = "Text";
            dbclient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").ReplaceOne(filter, data);
            return get();
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

            var filter = Builders<TextModel>.Filter.Eq("_id", id);


            dbClient.GetDatabase("Gojs").GetCollection<TextModel>("Texte").DeleteOne(filter);

            return get();
        }
    }
}
