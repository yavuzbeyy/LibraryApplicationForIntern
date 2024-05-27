using Katmanli.Core.Interfaces.DataAccessInterfaces;
using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.Core.Response;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katmanli.API.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult Create(UserCreate userCreateModel)
        {
            var response = _userService.Create(userCreateModel);

            if (response.Success)
            {
                return Ok(response); 
            }

            return BadRequest(response.Message); 
        }

        [HttpDelete("DeleteBookRequest")]
        public IActionResult DeleteBookRequest(int id)
        {
            var response = _userService.DeleteBookRequest(id);

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
            var response = _userService.Delete(id);

            if (response.Success)
            {
                return Ok(response.Message); 
            }

            return BadRequest(response.Message); 
        }

        [Authorize(Policy = "Admin")]
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserLoginDto userLoginModel)
        {
            var response = _userService.Login(userLoginModel);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("GetAllRequests")]
        public IActionResult GetAllRequests()
        {
            var response = _userService.GetAllRequests();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("CreateBookRequest")]
        public IActionResult CreateBookRequest(BookRequestCreateDto bookRequest)
        {
            var response = _userService.CreateBookRequest(bookRequest);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string username)
        {
            var response = _userService.PasswordReminder(username);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("CreateUserGroup")]
        public IActionResult CreateUserGroup(string groupName)
        {
            var response = _userService.CreateGroupforMessages(groupName);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [HttpGet("GetAllGroups")]
        public IActionResult GetAllGroups()
        {
            var response = _userService.ListAllMessageGroups();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("DeleteGroupById")]
        public IActionResult DeleteGroupById(int groupId)
        {
            var response = _userService.DeleteGroupById(groupId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("AddUserToGroup")]
        public IActionResult AddUserToGroup(string username,int groupId)
        {
            var response = _userService.addUserToGroup(username,groupId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response.Message);
        }
    }
}
