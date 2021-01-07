using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Logic.Services
{
    public class SyllabusService : DocumentInAcademicYearService<Syllabus>, ISyllabusService
    {
        public SyllabusService(SyllabusManagerDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Syllabus> Latest(string fos, string spec, string year)
        {
            Syllabus syllabus = await _dbSet.Include(s => s.FieldOfStudy)
                                       .Include(s => s.Specialization)
                                       .Include(s => s.Description)
                                       .Include(s => s.SubjectDescriptions)
                                       .ThenInclude(sd => sd.Subject)
                                       .OrderByDescending(s => s.Version)
                                       .FirstOrDefaultAsync(s =>
                                           s.FieldOfStudy.Code == fos
                                           && s.Specialization.Code == spec
                                           && s.AcademicYear == year
                                           && !s.IsDeleted);
            if (syllabus is null)
            {
                syllabus = new Syllabus
                {
                    FieldOfStudy = _dbContext.FieldsOfStudies.Include(f => f.Specializations).FirstOrDefault(f => f.Code == fos),
                    Specialization = _dbContext.Specializations.Find(spec),
                    AcademicYear = year,
                    Version = "new"
                };
            }

            return syllabus;
        }

        /// <summary>
        /// Zapisuje obiekt w najnowsze wersji
        /// </summary>
        /// <param name="syllabus"></param>
        /// <returns></returns>
        public async Task<Syllabus> Save(Syllabus syllabus, SyllabusManagerUser user)
        {
            if (syllabus.Id == Guid.Empty)
            {
                syllabus.Version = DateTime.UtcNow.ToString("yyyyMMdd") + "01";
                syllabus.CreationDate = DateTime.Now;
                syllabus.AuthorName = user.Name;
            }
            else
                syllabus.Version = IncreaseVersion(syllabus.Version);

            syllabus.Id = Guid.NewGuid();

            syllabus.SubjectDescriptions.ForEach(sd =>
            {
                sd.Id = Guid.NewGuid();
                if (sd.Subject != null)
                {
                    Data.Models.Subjects.Subject subj = _dbContext.Subjects.Find(sd.Subject.Code);
                    sd.Subject = subj;
                }
            });

            syllabus.Description.Id = Guid.NewGuid();

            Data.Models.FieldOfStudies.FieldOfStudy fos = _dbContext.FieldsOfStudies.Find(syllabus.FieldOfStudy.Code);
            syllabus.FieldOfStudy = fos;
            syllabus.Specialization = _dbContext.Specializations.Find(syllabus.Specialization.Code);

            syllabus.IsDeleted = false;

            await _dbSet.AddAsync(syllabus);
            await _dbContext.SaveChangesAsync();

            return syllabus;
        }

        /// <summary>
        /// Zapisuje obiekt w najnowszej wersji ale jako inny obiekt o podanych parametrach
        /// </summary>
        /// <param name="fos"></param>
        /// <param name="spec"></param>
        /// <param name="year"></param>
        /// <param name="syllabus"></param>
        /// <returns></returns>
        public async Task<Syllabus> SaveAs(string fosCode, string specCode, string academicYear, Syllabus syllabus, SyllabusManagerUser user)
        {
            var currentSyllabus = await Latest(fosCode, specCode, academicYear);

            currentSyllabus.ThesisCourse = syllabus.ThesisCourse;
            currentSyllabus.Description = syllabus.Description;
            currentSyllabus.IntershipType = syllabus.IntershipType;
            currentSyllabus.OpinionDeadline = syllabus.OpinionDeadline;
            currentSyllabus.ScopeOfDiplomaExam = syllabus.ScopeOfDiplomaExam;
            currentSyllabus.SubjectDescriptions = syllabus.SubjectDescriptions;

            return await Save(currentSyllabus, user);
        }

        /// <summary>
        /// Pobiera najnowszą wersję z obiektu o podanych parametrach i zapisuje jej kopię jako najnowsza wersja obiektu
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <param name="fos"></param>
        /// <param name="spec"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<Syllabus> ImportFrom(Guid currentDocId, string fosCode, string specCode, string academicYear, SyllabusManagerUser user)
        {
            Syllabus currentSyllabus = await _dbSet.AsNoTracking()
                                              .Include(s => s.FieldOfStudy)
                                              .Include(s => s.Specialization)
                                              .Include(s => s.SubjectDescriptions)
                                              .ThenInclude(sd => sd.Subject)
                                              .Include(s => s.Description)
                                              .FirstOrDefaultAsync(s =>
                                                                       s.Id == currentDocId
                                                                    && !s.IsDeleted);

            Syllabus syllabus = await _dbSet.AsNoTracking()
                                            .Include(s => s.FieldOfStudy)
                                            .Include(s => s.Specialization)
                                            .Include(s => s.SubjectDescriptions)
                                            .ThenInclude(sd => sd.Subject)
                                            .Include(s => s.Description)
                                            .FirstOrDefaultAsync(s =>
                                                                     s.FieldOfStudy.Code == fosCode
                                                                  && s.AcademicYear == academicYear
                                                                  && s.Specialization.Code == specCode
                                                                  && !s.IsDeleted);

            if (currentSyllabus is null || syllabus?.FieldOfStudy is null || syllabus.Specialization is null)
                return null;

            currentSyllabus.ThesisCourse = syllabus.ThesisCourse;
            currentSyllabus.Description = syllabus.Description;
            currentSyllabus.IntershipType = syllabus.IntershipType;
            currentSyllabus.OpinionDeadline = syllabus.OpinionDeadline;
            currentSyllabus.ScopeOfDiplomaExam = syllabus.ScopeOfDiplomaExam;
            currentSyllabus.SubjectDescriptions = syllabus.SubjectDescriptions;

            return await Save(currentSyllabus, user);
        }

        /// <summary>
        /// Pobiera historię wersji (jako lista string z nazwami wersji)
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <returns></returns>
        public async Task<List<string>> History(Guid id)
        {
            Syllabus syllabus = await _dbSet.Include(s => s.FieldOfStudy)
                                            .Include(s => s.Specialization)
                                            .Include(s => s.SubjectDescriptions)
                                            .ThenInclude(sd => sd.Subject)
                                            .Include(s => s.Description)
                                            .FirstOrDefaultAsync(s =>
                                                                     s.Id == id
                                                                  && !s.IsDeleted);

            List<string> versions = await _dbSet.Include(s => s.FieldOfStudy)
                                                .Include(s => s.Specialization)
                                                .Where(s =>
                                                           s.AcademicYear == syllabus.AcademicYear
                                                        && s.FieldOfStudy == syllabus.FieldOfStudy
                                                           && s.Specialization == syllabus.Specialization
                                                           && !s.IsDeleted)
                                                .Select(s => s.Version)
                                                .OrderBy(s => s)
                                                .ToListAsync();
            return versions;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization).FirstOrDefault(f => f.Id == id);

            var syllabuses = await _dbSet.Include(s => s.FieldOfStudy)
                .Include(s => s.Specialization)
                .Where(s =>
                    s.FieldOfStudy == entity.FieldOfStudy
                    && s.Specialization == entity.Specialization
                    && s.AcademicYear == entity.AcademicYear
                    && !s.IsDeleted).ToListAsync();

            syllabuses.ForEach(s => s.IsDeleted = true);
            var state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }
    }
}

