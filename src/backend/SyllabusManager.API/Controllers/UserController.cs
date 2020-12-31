using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Logic.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using User = SyllabusManager.Logic.Models.DTO.UserDTO;

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
            User result = await _userService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ByEmail(string email)
        {
            User result = await _userService.GetByEmailAsync(email);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            List<User> result = await _userService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            User result = await _userService.UpdateAsync(user);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            User result = await _userService.UpdateAsync(user);
            if (result == null)
                return BadRequest();
            return Created(result.Id, result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool result = await _userService.DeleteAsync(id);
            if (result == false)
                return NotFound();
            return Ok();
        }
    }
}
