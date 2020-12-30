using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SyllabusManager.API.Extensions;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Extensions;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models;
using SyllabusManager.Logic.Models.DTO;
using SyllabusManager.Logic.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<UserDTO> RegisterUser(RegistrationModel registrationModel)
        {

            Log.Information($"RegisterUser - attempting for: {registrationModel.Email}");

            SyllabusManagerUser syllabusUser = new SyllabusManagerUser
            {
                Name = $"{registrationModel.Name} {registrationModel.Surname}",
                Email = registrationModel.Email
            };
            await _userManager.CreateAsync(syllabusUser, registrationModel.Password);
            await _userManager.AddToRolesAsync(syllabusUser, registrationModel.Roles);
            List<string> roles = (await _userManager.GetRolesAsync(syllabusUser)).ToList();

            return syllabusUser.MakeDto(roles);

        }

        public async Task<UserCredentialsModel> Login(SyllabusManagerUser user)
        {
            Log.Information($"Login - attempting to log in for user {user.Email}");

            IList<string> roles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken token = MakeToken(user, roles);
            return await MakeUserCredentials(token, user);


        }

        private JwtSecurityToken MakeToken(SyllabusManagerUser user, IList<string> roles)
        {
            List<Claim> claims = MakeClaims(user, roles);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Value.Secret));

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
