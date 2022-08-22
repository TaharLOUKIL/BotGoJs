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
            BotModel bot = new BotModel(_configuration);
            return bot.LoadAll();
        }

        [HttpPost]
        public JsonResult Post(BotModel data)
        {
            BotModel bot = new BotModel(_configuration);
            bot.Save(data);
            return get();

        }

        [HttpPut]
        public JsonResult Put(BotModel data)
        {
            BotModel bot = new BotModel(_configuration);
            bot.Update(data);
            return get();
        }

    }
}
