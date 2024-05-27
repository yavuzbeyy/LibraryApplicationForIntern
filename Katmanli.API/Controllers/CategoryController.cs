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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("Create")]
        public IActionResult Create(CategoryCreate categoryCreateModel)
        {
            var response = _categoryService.Create(categoryCreateModel);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("GetCategoryById")]
        public IActionResult GetCategoryById(int id)
        {
            var response = _categoryService.FindById(id);

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
            var response = _categoryService.Delete(id);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("ListAll")]
        public IActionResult List()
        {
            var response = _categoryService.ListAll();

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update(CategoryUpdate categoryUpdateModel)
        {
            var response = _categoryService.Update(categoryUpdateModel);

            if (response.Success) 
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
