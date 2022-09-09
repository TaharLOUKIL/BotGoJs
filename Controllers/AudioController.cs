using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        [HttpGet]
        public JsonResult get()
        {
            AudioModel audio = new AudioModel(_configuration);

            return audio.LoadAll();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            var formCollection = await Request.ReadFormAsync();
            AudioModel audio = new AudioModel(_configuration);
            audio.Save(formCollection);
            return audio.LoadAll();
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<JsonResult> update()
        {
            var formCollection = await Request.ReadFormAsync();
            AudioModel audio = new AudioModel(_configuration);
            audio.Update(formCollection);
            return audio.LoadAll();
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            AudioModel audio = new AudioModel(_configuration);
            audio.Delete(id);
            return get();
        }
    }
}