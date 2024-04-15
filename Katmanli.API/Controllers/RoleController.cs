using Katmanli.Core.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katmanli.API.Controllers
{
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
    }
}
