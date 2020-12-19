using Microsoft.EntityFrameworkCore;

namespace SyllabusManager.Data.ProviderContexts
{
    public class SqlServerSyllabusManagerDbContext : SyllabusManagerDbContext
    {
        public SqlServerSyllabusManagerDbContext(DbContextOptions<SqlServerSyllabusManagerDbContext> options) : base(options) { }
    }
}
