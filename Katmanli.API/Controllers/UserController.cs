using Katmanli.Core.Interfaces.DataAccessInterfaces;
using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katmanli.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("ListAll")]
        public IActionResult List()
        {
            var getUsers = _userService.ListAll();
            return Ok(getUsers);
        }


        [HttpPost("Create")]
        public IActionResult Create(UserCreate userCreateModel) 
        {
           var kullaniciOlustur = _userService.Create(userCreateModel);
           return Ok(kullaniciOlustur);
        }
    }
}
