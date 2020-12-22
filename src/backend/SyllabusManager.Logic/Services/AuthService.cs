using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SyllabusManager.Logic.Settings;
using Serilog;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.API.Extensions;

namespace SyllabusManager.Logic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<SyllabusManagerUser> _userManager;
        private readonly IOptions<AuthorizationOptions> _authOptions;

        public AuthService(UserManager<SyllabusManagerUser> userManager, IOptions<AuthorizationOptions> authOptions)
        {
            _userManager = userManager;
            _authOptions = authOptions;
        }

        public async Task<IdentityResult> RegisterUser(RegistrationModel registrationModel)
        {

            Log.Information($"RegisterUser - attempting for: {registrationModel.Email}");

            SyllabusManagerUser syllabusUser = new SyllabusManagerUser
            {
                Name = $"{registrationModel.Name} {registrationModel.Surname}",
                Email = registrationModel.Email
            };

            return await _userManager.CreateAsync(syllabusUser, registrationModel.Password);

        }

        public async Task<UserCredentialsModel> Login(SyllabusManagerUser user)
        {
            Log.Information($"Login - attempting to log in for user {user.Email}");

            var roles = await _userManager.GetRolesAsync(user);
            var token = MakeToken(user, roles);
            return await MakeUserCredentials(token, user);


        }

        private JwtSecurityToken MakeToken(SyllabusManagerUser user, IList<string> roles)
        {
            var claims = MakeClaims(user, roles);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Value.Secret));

            return new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(_authOptions.Value.Expiration),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        }

        private List<Claim> MakeClaims(SyllabusManagerUser user, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRoles(roles);
            return claims;
        }


        private async Task<UserCredentialsModel> MakeUserCredentials(JwtSecurityToken token, SyllabusManagerUser user)
        {
            return new UserCredentialsModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Account = new AccountModel()
                {
                    Id = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                }
            };
        }

    }
}
