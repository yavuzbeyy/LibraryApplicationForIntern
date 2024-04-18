using Katmanli.Core.SharedLibrary;
using Katmanli.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katmanli.API.Controllers
{
        [Route("file")]
        [ApiController]
        public class UploadController : ControllerBase
        {
            private readonly IUploadService _uploadService;

            public UploadController(IUploadService uploadService)
            {
                _uploadService = uploadService;
            }


        [HttpGet("GetImageByBookId")]
        [RequestFormLimits(MultipartBodyLengthLimit = 30L * 1024 * 1024)] // 30 MB
        [RequestSizeLimit(30L * 1024 * 1024)] // 30 MB
        public IActionResult GetFile(int bookId)
        {
            try
            {
               var response = _uploadService.GetFile(bookId);

               return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        [HttpPost]
            [Route("Upload")]
            public async Task<IActionResult> UploadFile(IFormFile? imageFile, [FromForm] int bookId)
            {
                try
                {
                    var response = _uploadService.UploadFile(imageFile, bookId);

                    return Ok("Yükleme İşlemi Başarılı");
                }
                catch (Exception e)
                {
                    return BadRequest((e.StackTrace));
                }

            }

        }
    }

