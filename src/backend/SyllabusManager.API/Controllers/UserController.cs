using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using User = SyllabusManager.Data.Models.User.SyllabusManagerUser;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers
{
    
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ById(string id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            var result = await _userService.UpdateAsync(user);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            var result = await _userService.UpdateAsync(user);
            if (result == null)
                return BadRequest();
            return Created(result.Id, result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result == false)
                return NotFound();
            return Ok();
        }
    }
}
