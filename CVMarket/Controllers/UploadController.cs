using CVMarket.API.Attributes;
using CVMarket.API.Base;
using CVMarket.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CVMarket.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UploadController : ControllersBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var response = await _uploadService.UploadFile(file);

            return APIResponse(response);
        }

        [HttpGet, Route("")]
        public IActionResult GetFiles()
        {
            var response = _uploadService.GetFiles();

            return APIResponse(response);
        }

        [HttpGet, Route("getByPath")]
        public IActionResult GetFile([FromForm] string path)
        {
            try
            {
                var stream = new FileStream(path, FileMode.Open);
                return File(stream, "application/octet-stream", "{{filename.ext}}");
            }
            catch (Exception e)
            {
                return NotFound(new { Message = e.Message });
            }
        }
    }
}
