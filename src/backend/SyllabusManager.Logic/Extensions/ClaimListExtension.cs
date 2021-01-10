using System.Collections.Generic;
using System.Security.Claims;

namespace SyllabusManager.API.Extensions
{
    public static class ClaimListExtension
    {
        public static void AddRoles(this List<Claim> claims, IList<string> roles)
        {
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }
    }
}
