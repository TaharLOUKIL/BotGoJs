using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public LocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult get()
        {
            LocationModel location = new LocationModel(_configuration);
            return location.LoadAll();
        }

        [HttpPost]
        public JsonResult Post(LocationModel data)
        {
            LocationModel location = new LocationModel(_configuration);
            location.Save(data);
            return get();
        }

        [HttpPut]
        public JsonResult Put(LocationModel data)
        {
            LocationModel location = new LocationModel(_configuration);
            location.Update(data);
            return get();
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            LocationModel location = new LocationModel(_configuration);
            location.Delete(id);
            return get();
        }
    }
}