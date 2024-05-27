using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
using Katmanli.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katmanli.API.Controllers
{
        [Authorize]
        [Route("file")]
        [ApiController]
        public class UploadController : ControllerBase
        {
            private readonly IUploadService _uploadService;

            public UploadController(IUploadService uploadService)
            {
                _uploadService = uploadService;
            }


        [HttpGet("GetImageByFotokey")]
        [RequestFormLimits(MultipartBodyLengthLimit = 30L * 1024 * 1024)] // 30 MB
        [RequestSizeLimit(30L * 1024 * 1024)] // 30 MB
        public async Task<IActionResult> GetFile(string filekey)
        {
            try
            {
                var file = await _uploadService.GetFile(filekey);

                if (file == null || file.Data.fileContent == null)
                {
                    return BadRequest(new ErrorResponse<string>(Messages.NotFound("dosya")));
                }

                var fileData = file.Data;


                // Dosyanın content-disposition başlığını inline olarak ayarlayın
                string sanitizedFileName = fileData.fileName
                .Replace("ğ", "g")
                .Replace("Ğ", "G")
                .Replace("ü", "u")
                .Replace("Ü", "U")
                .Replace("ş", "s")
                .Replace("Ş", "S")
                .Replace("ı", "i")
                .Replace("İ", "I")
                .Replace("ö", "o")
                .Replace("Ö", "O")
                .Replace("ç", "c")
                .Replace("Ç", "C")
                .Replace("\"", "\\\""); // Çift tırnakları kaçış karakteriyle değiştir

                sanitizedFileName = sanitizedFileName.Replace(" ", "_"); // Boşlukları alt çizgiyle değiştir, alternatif olarak kaldırabilirsiniz
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{sanitizedFileName}\"");


                // Dosya içeriğini döndürün
                return fileData.fileContent;
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse<string>(e.StackTrace));
            }
        }

            [Authorize(Policy = "Admin")]
            [HttpPost]
            [Route("Upload")]
            public async Task<IActionResult> UploadFile(IFormFile? imageFile)
            {
                try
                {
                    var response = _uploadService.UploadFile(imageFile);

                    return Ok(response);
                }
                catch (Exception e)
                {
                    return BadRequest((e.StackTrace));
                }

            }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [Route("UploadToFtp")]
        public async Task<IActionResult> UploadFileToFtp(IFormFile? imageFile, [FromForm] int bookId)
        {
            try
            {
                var response = _uploadService.UploadFileToFtpServer(imageFile, bookId);

                return Ok("Yükleme İşlemi Başarılı");
            }
            catch (Exception e)
            {
                return BadRequest((e.StackTrace));
            }
        }

        [HttpGet("GetImageByFotokeyFromRedis")]
        public async Task<IActionResult> GetFileFromRedis(string filekey)
        {
            try
            {
                var file = await _uploadService.GetFileFromRedis(filekey);

                if (file == null || file.Data.fileContent == null)
                {
                    return BadRequest(new ErrorResponse<string>(Messages.NotFound("dosya")));
                }

                var fileData = file.Data;


                // Dosyanın content-disposition başlığını inline olarak ayarlayın
                string sanitizedFileName = fileData.fileName
                .Replace("ğ", "g")
                .Replace("Ğ", "G")
                .Replace("ü", "u")
                .Replace("Ü", "U")
                .Replace("ş", "s")
                .Replace("Ş", "S")
                .Replace("ı", "i")
                .Replace("İ", "I")
                .Replace("ö", "o")
                .Replace("Ö", "O")
                .Replace("ç", "c")
                .Replace("Ç", "C")
                .Replace("\"", "\\\""); // Çift tırnakları kaçış karakteriyle değiştir

                sanitizedFileName = sanitizedFileName.Replace(" ", "_"); // Boşlukları alt çizgiyle değiştir, alternatif olarak kaldırabilirsiniz
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{sanitizedFileName}\"");


                // Dosya içeriğini döndürün
                return fileData.fileContent;
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse<string>(e.StackTrace));
            }
        }
    }
}


    

