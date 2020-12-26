﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers
{
    [ApiController]
    public class AuthController : AbstractApiController
    {
        private readonly IAuthService _authService;
        private readonly UserManager<SyllabusManagerUser> _userManager;

        public AuthController(IAuthService authService, UserManager<SyllabusManagerUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationModel registrationModel)
        {
            var result = await _authService.RegisterUser(registrationModel);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            SyllabusManagerUser user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
                return NotFound();

            return Ok(await _authService.Login(user));

        }
    }
}
