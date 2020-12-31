using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyllabusManager.Data.Models.User
{
    [NotMapped]
    public class SyllabusManagerRole : IdentityRole
    {
        public ICollection<SyllabusManagerUserRole> UserRoles { get; set; }
        public SyllabusManagerRole(string roleName) : base(roleName) { }
        public SyllabusManagerRole() : base() { }
    }
}
