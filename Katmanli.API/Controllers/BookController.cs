using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Katmanli.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Katmanli.API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService) 
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        //[Authorize(Policy = "Admin")]
        [HttpGet("ListAll")]
        public IActionResult List()
        {
            var response = _bookService.ListAll();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("ListBooksByCategoryId")]
        public IActionResult ListByCategoryId(int categoryId)
        {
            var response = _bookService.ListBooksByCategoryId(categoryId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("ListBooksByAuthorId")]
        public IActionResult ListBooksByAuthorId(int authorId)
        {
            var response = _bookService.ListBooksByAuthorId(authorId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("Create")]
        public IActionResult Create(BookCreate bookCreateModel)
        {
            var response = _bookService.Create(bookCreateModel);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        
        [HttpGet("GetBookById")]
        public IActionResult GetBookById(int id)
        {
            var response = _bookService.FindById(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var response = _bookService.Delete(id);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update(BookUpdate bookUpdateModel)
        {
            var response = _bookService.Update(bookUpdateModel);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPut("UpdateBookIsAvailable")]
        public IActionResult UpdateBookIsAvailable(BookUpdate bookUpdateModel)
        {
            var response = _bookService.UpdateIsAvailable(bookUpdateModel);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost("BookQueryWithAIModel")]
        public IActionResult BookQueryWithAIModel([FromBody] BookContentQueryDto bookContentQuery)
        {
            var response = _bookService.askQueryToAIModel(bookContentQuery.bookContent);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest();
        }
    }
}
