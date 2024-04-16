﻿using Katmanli.DataAccess.DTOs;
using Katmanli.Service.Interfaces;
using Katmanli.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katmanli.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost("Create")]
        public IActionResult Create(AuthorCreate authorCreateModel)
        {
            var response = _authorService.Create(authorCreateModel);

            if (response.Success)
            {
                return Ok(response.Message);
            }

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

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var response = _authorService.Delete(id);

            if (response.Success)
            {
                return Ok(response.Message);
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

        [HttpPut("Update")]
        public IActionResult Update()
        {
            return null;
        }
    }
}
