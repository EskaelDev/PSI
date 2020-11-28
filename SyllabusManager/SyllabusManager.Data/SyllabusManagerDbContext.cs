using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Data
{
    public class SyllabusManagerDbContext : IdentityDbContext<User>
    {
        public SyllabusManagerDbContext(DbContextOptions options) : base(options) { }
    }
}
