using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            VideoModel video = new VideoModel(_configuration);
            video.Delete(id);
            return get();
        }

        [HttpGet]
        public JsonResult get()
        {
            VideoModel video = new VideoModel(_configuration);
            return video.LoadAll();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            var formCollection = await Request.ReadFormAsync();
            VideoModel video = new VideoModel(_configuration);
            video.SaveAsync(formCollection);
            return video.LoadAll();
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<JsonResult> update()
        {
            var formCollection = await Request.ReadFormAsync();
            VideoModel video = new VideoModel(_configuration);
            video.Update(formCollection);
            return video.LoadAll();
        }
    }
}