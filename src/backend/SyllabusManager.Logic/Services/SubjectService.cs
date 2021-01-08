using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.ManyToMany;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Logic.Services
{
    public class SubjectService : DocumentInAcademicYearService<Subject>, ISubjectService
    {
        public SubjectService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager) : base(dbContext, userManager)
        {

        }

        public async Task<List<Subject>> GetAll(string fos, string spec, string year)
        {
            return await _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization)
                .OrderByDescending(s => s.Version)
                .Where(s =>
                    s.FieldOfStudy.Code == fos
                    && s.Specialization.Code == spec
                    && s.AcademicYear == year
                    && !s.IsDeleted).ToListAsync();
        }

        public async Task<List<Subject>> GetAllForUser(string fos, string spec, string year, SyllabusManagerUser user, bool onlyMy)
        {
            return await _dbSet.Include(s => s.FieldOfStudy)
                .ThenInclude(f => f.Supervisor)
                .Include(s => s.Specialization)
                .OrderByDescending(s => s.Version)
                .Where(s =>
                    s.FieldOfStudy.Code == fos
                    && s.Specialization.Code == spec
                    && s.AcademicYear == year
                    && (s.Supervisor.Id == user.Id || (!onlyMy && s.FieldOfStudy.Supervisor.Id == user.Id))
                    && !s.IsDeleted).ToListAsync();
        }

        public async Task<Subject> Latest(string fos, string spec, string code, string year)
        {
            var subject = await _dbSet.Include(s => s.FieldOfStudy)
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
                subject.Teachers = subject.SubjectsTeachers.Select(st => st.Teacher).ToList();
            }

            return subject;
        }


        public async Task<Subject> Save(Subject subject)
        {
            if (subject.Id == Guid.Empty)
            {
                var existing = await _dbSet.Include(s => s.FieldOfStudy)
                    .Include(s => s.Specialization)
                    .FirstOrDefaultAsync(s =>
                        s.AcademicYear == subject.AcademicYear
                        && s.FieldOfStudy.Code == subject.FieldOfStudy.Code
                        && s.Specialization.Code == subject.Specialization.Code
                        && s.Code == subject.Code
                        && !s.IsDeleted);

                if (existing != null) return null;

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

            subject.IsDeleted = false;

            await _dbSet.AddAsync(subject);
            await _dbContext.SaveChangesAsync();

            return subject;
        }

        public async Task<Subject> ImportFrom(Guid currentDocId, string fosCode, string specCode, string code, string academicYear)
        {
            var currentSubject = await _dbSet.AsNoTracking()
                                              .Include(s => s.FieldOfStudy)
                                              .Include(s => s.Specialization)
                                              .FirstOrDefaultAsync(s =>
                                                                       s.Id == currentDocId
                                                                    && !s.IsDeleted);

            var subject = await _dbSet.AsNoTracking()
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

            if (currentSubject is null || subject?.FieldOfStudy is null || subject.Specialization is null)
                return null;

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
            var subject = await _dbSet.Include(s => s.FieldOfStudy)
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
                                                .Select(s => s.Version)
                                                .OrderBy(s => s)
                                                .ToListAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization).FirstOrDefault(f => f.Id == id);

            var subjects = await _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization)
                .Where(s =>
                    s.FieldOfStudy == entity.FieldOfStudy
                    && s.Specialization == entity.Specialization
                    && s.Code == entity.Code
                    && s.AcademicYear == entity.AcademicYear
                    && !s.IsDeleted).ToListAsync();
            
            subjects.ForEach(s => s.IsDeleted = true);
            var state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }
    }
}
