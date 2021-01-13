using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.ManyToMany;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public class SubjectService : DocumentInAcademicYearService<Subject>, ISubjectService
    {
        public SubjectService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager) : base(dbContext, userManager)
        {

        }

        public async Task<List<Subject>> GetAll(string fos, string spec, string year, SyllabusManagerUser user)
        {
            IEnumerable<Guid> latestIds = _dbSet.AsNoTracking().Where(s =>
                s.FieldOfStudy.Code == fos
                && s.Specialization.Code == spec
                && s.AcademicYear == year
                && !s.IsDeleted).ToList()
                .GroupBy(s => new { s.FieldOfStudy, s.Specialization, s.AcademicYear })
                .Select(g => g.OrderByDescending(s => s.Version)
                    .First().Id);

            List<Subject> result = await _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization)
                .Include(s => s.Supervisor)
                .Include(s => s.SubjectsTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => latestIds.Contains(s.Id)).ToListAsync();

            return result.Select(r =>
            {
                r.IsSupervisor = r.Supervisor.Id == user.Id;
                r.IsTeacher = r.SubjectsTeachers.Any(t => t.Teacher.Id == user.Id);
                r.SubjectsTeachers = new List<SubjectTeacher>();
                return r;
            }).ToList();
        }

        public async Task<Subject> Latest(string fos, string spec, string code, string year, SyllabusManagerUser user)
        {
            Subject subject = await _dbSet.Include(s => s.FieldOfStudy)
                                       .Include(s => s.Specialization)
                                       .Include(s => s.Supervisor)
                                       .Include(s => s.CardEntries)
                                       .ThenInclude(ce => ce.Entries)
                                       .Include(s => s.LearningOutcomeEvaluations)
                                       .Include(s => s.Lessons)
                                       .ThenInclude(l => l.ClassForms)
                                       .Include(s => s.Literature)
                                       .Include(s => s.SubjectsTeachers)
                                       .ThenInclude(st => st.Teacher)
                                       .OrderByDescending(s => s.Version)
                                       .FirstOrDefaultAsync(s =>
                                           s.FieldOfStudy.Code == fos
                                           && s.Specialization.Code == spec
                                           && s.Code == code
                                           && s.AcademicYear == year
                                           && !s.IsDeleted);

            if (subject != null)
            {
                if (subject.FieldOfStudy is null || subject.Specialization is null) return null;

                subject.Teachers = subject.SubjectsTeachers.Select(st =>
                {
                    st.Teacher.SubjectsTeachers = new List<SubjectTeacher>();
                    return st.Teacher;
                }).ToList();
                subject.SubjectsTeachers = new List<SubjectTeacher>();
                subject.IsSupervisor = subject.Supervisor.Id == user.Id;
                subject.IsTeacher = subject.Teachers.Any(t => t.Id == user.Id);
            }

            return subject;
        }


        public async Task<int> Save(Subject subject)
        {
            if (subject.Id == Guid.Empty)
            {
                Subject existing = await _dbSet.Include(s => s.FieldOfStudy)
                    .Include(s => s.Specialization)
                    .FirstOrDefaultAsync(s =>
                        s.AcademicYear == subject.AcademicYear
                        && s.FieldOfStudy.Code == subject.FieldOfStudy.Code
                        && s.Specialization.Code == subject.Specialization.Code
                        && s.Code == subject.Code
                        && !s.IsDeleted);

                if (existing != null) return 2;

                subject.Version = DateTime.UtcNow.ToString("yyyyMMdd") + "01";
                subject.CardEntries.Add(new CardEntries()
                {
                    Type = SubjectCardEntryType.Goal,
                    Name = "Cele przedmiotu"
                });
                subject.CardEntries.Add(new CardEntries()
                {
                    Type = SubjectCardEntryType.Prerequisite,
                    Name = "Wymagania wstępne w zakresie wiedzy, umiejętności i innych kompetencji"
                });
                subject.CardEntries.Add(new CardEntries()
                {
                    Type = SubjectCardEntryType.TeachingTools,
                    Name = "Stosowane narzędzia dydaktyczne"
                });
            }
            else
                subject.Version = IncreaseVersion(subject.Version);

            subject.Id = Guid.NewGuid();

            subject.Literature.ForEach(l => l.Id = Guid.NewGuid());
            subject.LearningOutcomeEvaluations.ForEach(l => l.Id = Guid.NewGuid());
            subject.Lessons.ForEach(l =>
            {
                l.Id = Guid.NewGuid();
                l.ClassForms.ForEach(cf => cf.Id = Guid.NewGuid());
            });
            subject.CardEntries.ForEach(l =>
            {
                l.Id = Guid.NewGuid();
                l.Entries.ForEach(e => e.Id = Guid.NewGuid());
            });
            subject.SubjectsTeachers.Clear();
            subject.Teachers.ForEach(t =>
            {
                subject.SubjectsTeachers.Add(new SubjectTeacher()
                {
                    Teacher = _dbContext.Users.Find(t.Id),
                    TeacherId = t.Id
                });
            });

            subject.FieldOfStudy = _dbContext.FieldsOfStudies.Find(subject.FieldOfStudy.Code);
            subject.Specialization = _dbContext.Specializations.Find(subject.Specialization.Code);
            subject.Supervisor = _dbContext.Users.Find(subject.Supervisor.Id);

            if (subject.FieldOfStudy is null || subject.Specialization is null || subject.Supervisor is null)
                return 1;

            subject.IsDeleted = false;

            await _dbSet.AddAsync(subject);
            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> ImportFrom(Guid currentDocId, string fosCode, string specCode, string code, string academicYear)
        {
            Subject currentSubject = await _dbSet.AsNoTracking()
                                              .Include(s => s.FieldOfStudy)
                                              .Include(s => s.Specialization)
                                              .FirstOrDefaultAsync(s =>
                                                                       s.Id == currentDocId
                                                                    && !s.IsDeleted);

            Subject subject = await _dbSet.AsNoTracking()
                                            .Include(s => s.FieldOfStudy)
                                            .Include(s => s.Specialization)
                                            .Include(s => s.CardEntries)
                                            .ThenInclude(ce => ce.Entries)
                                            .Include(s => s.LearningOutcomeEvaluations)
                                            .Include(s => s.Lessons)
                                            .ThenInclude(l => l.ClassForms)
                                            .Include(s => s.Literature)
                                            .Include(s => s.SubjectsTeachers)
                                            .ThenInclude(st => st.Teacher)
                                            .FirstOrDefaultAsync(s =>
                                                                     s.FieldOfStudy.Code == fosCode
                                                                  && s.AcademicYear == academicYear
                                                                  && s.Specialization.Code == specCode
                                                                  && !s.IsDeleted);

            if (currentSubject is null || subject is null) return 1;

            currentSubject.NamePl = subject.NamePl;
            currentSubject.NameEng = subject.NameEng;
            currentSubject.ModuleType = subject.ModuleType;
            currentSubject.KindOfSubject = subject.KindOfSubject;
            currentSubject.Language = subject.Language;
            currentSubject.TypeOfSubject = subject.TypeOfSubject;
            currentSubject.Literature = subject.Literature;
            currentSubject.Lessons = subject.Lessons;
            currentSubject.LearningOutcomeEvaluations = subject.LearningOutcomeEvaluations;
            currentSubject.CardEntries = subject.CardEntries;
            currentSubject.Teachers = subject.Teachers;

            return await Save(currentSubject);
        }

        public async Task<List<string>> History(Guid id)
        {
            Subject subject = await _dbSet.Include(s => s.FieldOfStudy)
                                            .Include(s => s.Specialization)
                                            .FirstOrDefaultAsync(s =>
                                                                     s.Id == id
                                                                  && !s.IsDeleted);

            return await _dbSet.Include(s => s.FieldOfStudy)
                                .Include(s => s.Specialization)
                                                .Where(s =>
                                                           s.AcademicYear == subject.AcademicYear
                                                            && s.FieldOfStudy == subject.FieldOfStudy
                                                           && s.Specialization == subject.Specialization
                                                           && s.Code == subject.Code
                                                        && !s.IsDeleted)
                                                .OrderByDescending(s => s.Version)
                                                .Select(s => $"{s.Id}:{s.Version}")
                                                .ToListAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            Subject entity = _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization).FirstOrDefault(f => f.Id == id);

            List<Subject> subjects = await _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization)
                .Where(s =>
                    s.FieldOfStudy == entity.FieldOfStudy
                    && s.Specialization == entity.Specialization
                    && s.Code == entity.Code
                    && s.AcademicYear == entity.AcademicYear
                    && !s.IsDeleted).ToListAsync();

            subjects.ForEach(s => s.IsDeleted = true);
            int state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }

        public string GetSupervisorId(Guid documentId)
        {
            return _dbSet.Include(s => s.Supervisor)
                .FirstOrDefault(s => s.Id == documentId)?.Supervisor?.Id;
        }

        public async Task<bool> Pdf(Guid id)
        {
            Subject subject = await _dbSet.Include(s => s.FieldOfStudy)
                                          .Include(s => s.Specialization)
                                          .Include(s => s.Supervisor)
                                          .Include(s => s.CardEntries)
                                            .ThenInclude(ce => ce.Entries)
                                          .Include(s => s.LearningOutcomeEvaluations)
                                          .Include(s => s.Lessons)
                                            .ThenInclude(l => l.ClassForms)
                                          .Include(s => s.Literature)
                                          .Include(s => s.SubjectsTeachers)
                                            .ThenInclude(st => st.Teacher)
                                          .OrderByDescending(s => s.Version)
                                          .FirstOrDefaultAsync(s =>
                                                                   s.Id == id
                                                                && !s.IsDeleted);
            if (subject is null)
                return false;

            PdfCreator pdf = new PdfCreator();
            pdf.Create(subject);

            return true;
        }



    }
}
