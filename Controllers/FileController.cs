using BotGoJs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BotGoJs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(string id)
        {
            FileModel file = new FileModel(_configuration);
            file.Delete(id);
            return get();
        }

        [HttpGet]
        public JsonResult get()
        {
            FileModel file = new FileModel(_configuration);
            return file.LoadAll();
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<JsonResult> upload()
        {
            var formCollection = await Request.ReadFormAsync();
            FileModel file = new FileModel(_configuration);
            file.SaveAsync(formCollection);
            return file.LoadAll();
        }

        [HttpPut, DisableRequestSizeLimit]
        public async Task<JsonResult> update()
        {
            var formCollection = await Request.ReadFormAsync();
            FileModel file = new FileModel(_configuration);
            file.Update(formCollection);
            return file.LoadAll();
        }
    }
}