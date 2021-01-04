using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                                       .Include(s => s.SubjectDescriptions)
                                       .ThenInclude(sd => sd.Subject)
                                       .Include(s => s.Description)
                                       .OrderByDescending(s => s.Version)
                                       .FirstOrDefaultAsync(s =>
                                                                s.FieldOfStudy.Id == fos
                                                             && s.Specialization.Id == spec
                                                             && s.AcademicYear == year
                                                             && !s.IsDeleted);
            if (syllabus is null)
            {
                syllabus = new Syllabus();
            }

            return syllabus;
        }

        /// <summary>
        /// Zapisuje obiekt w najnowsze wersji
        /// </summary>
        /// <param name="syllabus"></param>
        /// <returns></returns>
        public async Task<Syllabus> Save(Syllabus syllabus)
        {
            if (syllabus.Id == Guid.Empty)
                syllabus.Version = DateTime.UtcNow.ToString("yyyyMMdd") + "01";
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


            Data.Models.FieldOfStudies.FieldOfStudy fos = _dbContext.FieldsOfStudies.Find(syllabus.FieldOfStudy.Code);
            syllabus.FieldOfStudy = fos;

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
        public async Task<Syllabus> SaveAs(string fosCode, string specCode, string academicYear, Syllabus syllabus)
        {
            Data.Models.FieldOfStudies.FieldOfStudy fos = _dbContext.FieldsOfStudies.Find(fosCode);
            if (fos != null)
                syllabus.FieldOfStudy = fos;

            Data.Models.FieldOfStudies.Specialization spec = _dbContext.Specializations.Find(specCode);
            if (spec != null)
                syllabus.Specialization = spec;

            syllabus.AcademicYear = academicYear;

            return await Save(syllabus);
        }

        /// <summary>
        /// Pobiera najnowszą wersję z obiektu o podanych parametrach i zapisuje jej kopię jako najnowsza wersja obiektu
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <param name="fos"></param>
        /// <param name="spec"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<Syllabus> ImportFrom(Guid currentDocId, string fosCode, string specCode, string academicYear)
        {
            Syllabus currentSyllabus;

            if (currentDocId == Guid.Empty)
                currentSyllabus = new Syllabus();
            else
                currentSyllabus = await _dbSet.AsNoTracking()
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

            if (syllabus is null || syllabus.FieldOfStudy is null || syllabus.Specialization is null)
                return null;

            currentSyllabus.FieldOfStudy = syllabus.FieldOfStudy;
            currentSyllabus.Specialization = syllabus.Specialization;

            return await Save(currentSyllabus);
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
                                                .Where(s =>
                                                           s.AcademicYear == syllabus.AcademicYear
                                                        && s.FieldOfStudy == syllabus.FieldOfStudy
                                                        && !s.IsDeleted)
                                                .Select(s => s.Version)
                                                .OrderBy(s => s)
                                                .ToListAsync();
            return versions;
        }
    }
}

