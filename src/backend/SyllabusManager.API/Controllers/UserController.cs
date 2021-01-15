using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using User = SyllabusManager.Logic.Models.DTO.UserDTO;

namespace SyllabusManager.API.Controllers
{
    [Authorize]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = UsersRoles.Admin)]
        public async Task<IActionResult> ById(string id)
        {
            User result = await _userService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);

        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = UsersRoles.Admin)]
        public async Task<IActionResult> ByEmail(string email)
        {
            User result = await _userService.GetByEmailAsync(email);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = UsersRoles.Admin)]
        public async Task<IActionResult> All()
        {
            List<User> result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = UsersRoles.Admin)]
        public async Task<IActionResult> Save([FromBody] User user)
        {
            User result = await _userService.AddOrUpdateAsync(user);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = UsersRoles.Admin)]
        public IActionResult ResetPassword(string id)
        {
            // todo: Reset password and send by email
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = UsersRoles.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            bool result = await _userService.DeleteAsync(id);
            if (result == false)
                return NotFound();
            return Ok();
        }
    }
}
