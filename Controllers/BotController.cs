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
    public class BotController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public BotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var dblist = dbclient.GetDatabase("Gojs").GetCollection<BotModel>("Bots").AsQueryable();
            return new JsonResult(dblist);
        }

        [HttpPost]
        public JsonResult Post(BotModel data)
        {

            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            data._id = ObjectId.GenerateNewId().ToString();
            dbclient.GetDatabase("Gojs").GetCollection<BotModel>("Bots").InsertOne(data);
            return new JsonResult(data);
        }

        [HttpPut]
        public JsonResult Put(BotModel data)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var filter = Builders<BotModel>.Filter.Eq("_id", data._id);
            dbclient.GetDatabase("Gojs").GetCollection<BotModel>("Bots").ReplaceOne(filter, data);
            return new JsonResult(data);
        }

    }
}
