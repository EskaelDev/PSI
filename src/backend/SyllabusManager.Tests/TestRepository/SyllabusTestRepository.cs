using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;

namespace SyllabusManager.Tests.TestRepository
{
    public class SyllabusTestRepository
    {
        public static SyllabusManagerDbContext GetInMemorySyllabusContext()
        {
            DbContextOptions<SyllabusManagerDbContext> options;
            var builder = new DbContextOptionsBuilder<SyllabusManagerDbContext>();
            builder.UseInMemoryDatabase("syllabus");
            options = builder.Options;
            SyllabusManagerDbContext syllabusContext = new SyllabusManagerDbContext(options);
            syllabusContext.Database.EnsureDeleted();
            syllabusContext.Database.EnsureCreated();
            return syllabusContext;
        }
    }
}
