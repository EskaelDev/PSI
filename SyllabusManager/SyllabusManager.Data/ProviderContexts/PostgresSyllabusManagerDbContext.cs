using Microsoft.EntityFrameworkCore;

namespace SyllabusManager.Data.ProviderContexts
{
    public class PostgresSyllabusManagerDbContext : SyllabusManagerDbContext
    {
        public PostgresSyllabusManagerDbContext(DbContextOptions<PostgresSyllabusManagerDbContext> options) : base(options) { }
    }
}
