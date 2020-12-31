using Microsoft.AspNetCore.Identity;

namespace SyllabusManager.Data.Models.User
{
    public class SyllabusManagerUserRole : IdentityUserRole<string>
    {
        public virtual SyllabusManagerUser User { get; set; }
        public virtual SyllabusManagerRole Role { get; set; }
    }
}
