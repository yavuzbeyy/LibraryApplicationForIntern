using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Katmanli.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Katmanli.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService) 
        {
            _bookService = bookService;
        }

        [HttpGet("ListAll")]
        public IActionResult List()
        {
            var getUsers = _bookService.ListAll();
            return Ok(getUsers);
        }

        [HttpPost("Create")]
        public IActionResult Create(BookCreate bookCreateModel)
        {
            var kullaniciOlustur = _bookService.Create(bookCreateModel);
            return Ok(kullaniciOlustur);
        }


        [HttpGet("GetBookById")]
        public IActionResult GetBookById(int id)
        {
            var getUsers = _bookService.FindById(id);
            return Ok(getUsers);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var deletedUser = _bookService.Delete(id);
            return Ok(deletedUser);
        }
    }
}
