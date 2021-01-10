using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Models.User;
using System.Security.Claims;
using System.Threading.Tasks;
using SyllabusManager.Logic.Models;

namespace SyllabusManager.API.Helpers
{
    public static class AuthenticationHelper
    {
        public static async Task<SyllabusManagerUser> GetAuthorizedUser(ClaimsPrincipal user,
            UserManager<SyllabusManagerUser> userManager)
        {
            try
            {
                var identity = user?.Identity as ClaimsIdentity;
                var id = identity?.FindFirst(ClaimTypes.Name)?.Value;
                return await userManager.FindByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }
    }
}
