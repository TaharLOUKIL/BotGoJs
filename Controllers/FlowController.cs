using BotGoJs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotPressController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public BotPressController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("gojsConnection"));
            var obj = new AnyTypeModel();

            obj.image = new List<ImageModel>();
            obj.image = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<ImageModel>("Image").AsQueryable().ToList();

            obj.file = new List<FileModel>();
            obj.file = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<FileModel>("Fichier").AsQueryable().ToList();

            obj.audio = new List<AudioModel>();
            obj.audio = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<AudioModel>("Audio").AsQueryable().ToList();
           
            obj.video = new List<VideoModel>();
            obj.video = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<VideoModel>("Video").AsQueryable().ToList();

            obj.location = new List<LocationModel>();
            obj.location = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<LocationModel>("Localisation").AsQueryable().ToList();

            obj.text = new List<TextModel>();
            obj.text = dbclient.GetDatabase(_configuration["Variable:Databasename"]).GetCollection<TextModel>("Text").AsQueryable().ToList();
            return new JsonResult(obj);
        }
    }
}
