using Katmanli.Core.Interfaces.DataAccessInterfaces;
using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.Response;
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
            var response = _userService.ListAll();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message); 
        }

        [HttpGet("GetUserByUsername")]
        public IActionResult GetUserByUsername(string username)
        {
            var response = _userService.GetUserByUsername(username);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [HttpGet("GetUserByUserId")]
        public IActionResult GetUserById(int id)
        {
            var response = _userService.FindById(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("Create")]
        public IActionResult Create(UserCreate userCreateModel)
        {
            var response = _userService.Create(userCreateModel);

            if (response.Success)
            {
                return Ok(response.Message); 
            }

            return BadRequest(response.Message); 
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var response = _userService.Delete(id);

            if (response.Success)
            {
                return Ok(response.Message); 
            }

            return BadRequest(response.Message); 
        }
        [HttpPut("Update")]
        public IActionResult Update(UserUpdate userUpdateModel)
        {
            var response = _userService.Update(userUpdateModel);

            if(response.Success)
            {  
                return Ok(response);
            }
            return BadRequest(response.Message);
        }
    }
}
