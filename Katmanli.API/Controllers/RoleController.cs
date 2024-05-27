using Katmanli.Core.Interfaces.ServiceInterfaces;
using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Katmanli.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Katmanli.DataAccess.DTOs.CategoryDTO;

namespace Katmanli.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) 
        {
            _roleService = roleService;
        }

        [HttpGet("ListAll")]
        public IActionResult List()
        {
            var getAllRoles = _roleService.ListAll();
            return Ok(getAllRoles);
        }

        [HttpGet("ListRolesByUser")]
        public IActionResult ListRolesByUser()
        {
            var getAllRoles = _roleService.ListRolesByUser();
            return Ok(getAllRoles);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var response = _roleService.Delete(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("Create")]
        public IActionResult Create(RoleCreate roleCreateModel)
        {
            var response = _roleService.Create(roleCreateModel);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
