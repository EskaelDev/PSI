using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Data.Models.ManyToMany;
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

        //public DbSet<SubjectTeacher> SubjectTeachers { get; set; }

        public SyllabusManagerDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubjectTeacher>().HasKey(sc => new { sc.SubjectId, sc.TeacherId });

            modelBuilder.Entity<SubjectTeacher>()
                .HasOne(st => st.Subject)
                .WithMany(s => s.SubjectsTeachers)
                .HasForeignKey(st => st.SubjectId);


            modelBuilder.Entity<SubjectTeacher>()
                .HasOne(st => st.Teacher)
                .WithMany(u => u.SubjectsTeachers)
                .HasForeignKey(st => st.TeacherId);

            modelBuilder.Entity<Subject>().Ignore(s => s.Teachers);
        }
    }
}
