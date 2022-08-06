using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public TextController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult get()
        {
            TextModel text = new TextModel();
            return text.LoadAll();
        }

        [HttpPost]
        public JsonResult Post(TextModel data)
        {
            TextModel text = new TextModel();
            text.Save(data);
            return get();
        }

        [HttpPut]
        public JsonResult Put(TextModel data)
        {
            TextModel text = new TextModel();
            text.Update(data);
            return get();
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            TextModel text = new TextModel();
            text.Delete(id);
            return get();
        }
    }
}