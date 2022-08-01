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
    public class LocalisationController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public LocalisationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").AsQueryable();
            return new JsonResult(dblist);
        }

        [HttpPost]
        public JsonResult Post(LocationModel data)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            data._id = ObjectId.GenerateNewId().ToString();
            data.type = "Location";
            dbclient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").InsertOne(data);
            return get();
        }

        [HttpPut]
        public JsonResult Put(LocationModel data)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var filter = Builders<LocationModel>.Filter.Eq("_id", data._id);
            data.type = "Location";
            dbclient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").ReplaceOne(filter, data);
            return get();
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));

            var filter = Builders<LocationModel>.Filter.Eq("_id", id);


            dbClient.GetDatabase("Gojs").GetCollection<LocationModel>("Localisation").DeleteOne(filter);

            return get();
        }
    }
}
