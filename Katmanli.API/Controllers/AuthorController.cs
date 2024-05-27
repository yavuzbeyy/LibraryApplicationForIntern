using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Katmanli.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Katmanli.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;  

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("Create")]
        public IActionResult Create(AuthorCreate authorCreateModel)
        {
            var response = _authorService.Create(authorCreateModel);
            Log.Information("Create method called with {AuthorCreateModel}", authorCreateModel);
            if (response.Success)
            {
                Log.Information("Create method successfully executed");
                return Ok(response);
            }
            Log.Error("Create method failed with message: {Message}", response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("GetAuthorById")]
        public IActionResult GetAuthorById(int id)
        {
            var response = _authorService.FindById(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var response = _authorService.Delete(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [HttpGet("ListAll")]
        public IActionResult List()
        {
            var response = _authorService.ListAll();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update(AuthorUpdate authorUpdateModel)
        {
            var response = _authorService.Update(authorUpdateModel);

            if (response.Success) 
            {
                return Ok(response);   
            }

            return BadRequest();
        }
    }
}
