using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            ImageModel image = new ImageModel(_configuration);
            image.Delete(id);
            return get();
        }

        [HttpGet]
        public JsonResult get()
        {
            ImageModel image = new ImageModel(_configuration);
            return image.LoadAll();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            var formCollection = await Request.ReadFormAsync();
            ImageModel image = new ImageModel(_configuration);
            image.SaveAsync(formCollection);
            return image.LoadAll();
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<JsonResult> update()
        {
            var formCollection = await Request.ReadFormAsync();
            ImageModel image = new ImageModel(_configuration);
            image.Update(formCollection);
            return image.LoadAll();
        }
    }
}