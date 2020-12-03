using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Data
{
    public class SyllabusManagerDbContext : IdentityDbContext<SyllabusManagerUser>
    {
        public DbSet<FieldOfStudy> FieldsOfStudies { get; set; }
        public DbSet<LearningOutcomeDocument> LearningOutcomeDocuments { get; set; }
        public DbSet<Syllabus> Syllabuses { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public SyllabusManagerDbContext(DbContextOptions options) : base(options) { }
    }
}
