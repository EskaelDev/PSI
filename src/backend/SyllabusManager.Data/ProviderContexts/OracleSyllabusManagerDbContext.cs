using Microsoft.EntityFrameworkCore;

namespace SyllabusManager.Data.ProviderContexts
{
    public class OracleSyllabusManagerDbContext : SyllabusManagerDbContext
    {
        public OracleSyllabusManagerDbContext(DbContextOptions<OracleSyllabusManagerDbContext> options) : base(options) { }
    }
}
