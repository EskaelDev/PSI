using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Models;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Helpers
{
    public static class AuthorizationHelper
    {
        public static async Task<bool> CheckIfAdmin(SyllabusManagerUser user,
            UserManager<SyllabusManagerUser> userManager)
        {
            var roles = await userManager.GetRolesAsync(user);
            return roles.Contains(UsersRoles.Admin);
        }
    }
}
