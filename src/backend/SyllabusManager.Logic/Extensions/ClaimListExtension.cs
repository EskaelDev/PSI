using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SyllabusManager.API.Extensions
{
    public static class ClaimListExtension
    {
        public static void AddRoles(this List<Claim> claims, IList<string> roles)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }
    }
}
