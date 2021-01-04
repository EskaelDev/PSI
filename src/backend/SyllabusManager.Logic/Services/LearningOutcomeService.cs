using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public class LearningOutcomeService : DocumentInAcademicYearService<LearningOutcomeDocument>, ILearningOutcomeService
    {
        public LearningOutcomeService(SyllabusManagerDbContext dbContext) : base(dbContext)
        {

        }


        /// <summary>
        /// Zwraca obiekt LearningOutcomeDocument o najnowszej wersji dla podanych parametrów (jeżeli nie istnieje to zwraca nowy obiekt)
        /// </summary>
        /// <param name="fosCode">Kod FieldOfStudy(Kierunku studiów)</param>
        /// <param name="academicYear">Rok akademicki</param>
        /// <returns>LearningOutcomeDocument o najwyższej wersji</returns>
        public async Task<LearningOutcomeDocument> Latest(string fosCode, string academicYear)
        {
            LearningOutcomeDocument lodDb = await _dbSet.Include(lod => lod.FieldOfStudy)
                                                        .ThenInclude(fs => fs.Specializations)
                                                        .Include(lod => lod.LearningOutcomes)
                                                        .ThenInclude(lo => lo.Specialization)
                                                        .Where(lod =>
                                                                     lod.IsDeleted == false
                                                                  && lod.AcademicYear == academicYear
                                                                  && lod.FieldOfStudy.Code == fosCode)
                                                        .OrderByDescending(lod => lod.Version)
                                                        .FirstOrDefaultAsync();
            if (lodDb == null)
            {
                lodDb = new LearningOutcomeDocument();
                lodDb.AcademicYear = academicYear;
                lodDb.FieldOfStudy = _dbContext.FieldsOfStudies.Include(f => f.Specializations).FirstOrDefault(f => f.Code == fosCode);
                lodDb.Version = "new";
            }

            lodDb.FieldOfStudy?.Specializations.RemoveAll(s => s.IsDeleted);
            lodDb.FieldOfStudy?.Specializations?.ForEach(s => s.FieldOfStudy = null);

            return lodDb;
        }

        /// <summary>
        /// Zapisuje obiekt w najnowsze wersji
        /// </summary>
        /// <param name="learningOutcome"></param>
        /// <returns>Zapisany LearningOutcomeDocument</returns>
        public async Task<LearningOutcomeDocument> Save(LearningOutcomeDocument learningOutcome)
        {
            if (learningOutcome.Id == Guid.Empty)
                learningOutcome.Version = DateTime.UtcNow.ToString("yyyyMMdd") + "01";
            else
                learningOutcome.Version = IncreaseVersion(learningOutcome.Version);

            learningOutcome.Id = Guid.NewGuid();
            learningOutcome.LearningOutcomes.ForEach(lo =>
            {
                lo.Id = Guid.NewGuid();
                if (lo.Specialization != null)
                {
                    Data.Models.FieldOfStudies.Specialization spec = _dbContext.Specializations.Find(lo.Specialization.Code);
                    lo.Specialization = spec;
                }
            });
            Data.Models.FieldOfStudies.FieldOfStudy fos = _dbContext.FieldsOfStudies.Find(learningOutcome.FieldOfStudy.Code);
            learningOutcome.FieldOfStudy = fos;

            await _dbSet.AddAsync(learningOutcome);
            await _dbContext.SaveChangesAsync();

            return learningOutcome;
        }

        private string IncreaseVersion(string version)
        {
            string newVersion = DateTime.UtcNow.ToString("yyyyMMdd");

            if (version.Substring(0, 8) == newVersion)
            {
                string currentV = version.Substring(8);
                int newV = int.Parse(currentV) + 1;
                return version.Substring(0, 8) + newV.ToString("00");
            }

            return newVersion + "01";
        }

        /// <summary>
        /// Zapisuje obiekt w najnowszej wersji ale jako inny obiekt o podanych parametrach
        /// </summary>
        /// <param name="fosCode"></param>
        /// <param name="academicYear"></param>
        /// <param name="learningOutcome"></param>
        /// <returns></returns>
        public async Task<LearningOutcomeDocument> SaveAs(string fosCode, string academicYear, LearningOutcomeDocument learningOutcome)
        {
            Data.Models.FieldOfStudies.FieldOfStudy fos = _dbContext.FieldsOfStudies.Find(fosCode);
            if (fos != null)
                learningOutcome.FieldOfStudy = fos;

            learningOutcome.AcademicYear = academicYear;

            return await Save(learningOutcome);
        }


        /// <summary>
        /// Pobiera najnowszą wersję z obiektu o podanych parametrach i zapisuje jej kopię jako najnowsza wersja obiektu
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <param name="fosCode"></param>
        /// <param name="academicYear"></param>
        /// <returns></returns>
        public async Task<LearningOutcomeDocument> ImportFrom(string currentDocId, string fosCode, string academicYear)
        {
            LearningOutcomeDocument currentLod;

            if (currentDocId == Guid.Empty.ToString())
                currentLod = new LearningOutcomeDocument();
            else
                currentLod = await _dbSet.AsNoTracking().Include(lod => lod.FieldOfStudy)
                                                             .Include(lod => lod.LearningOutcomes).FirstOrDefaultAsync(l => l.Id.ToString() == currentDocId);

            LearningOutcomeDocument lod = await _dbSet.AsNoTracking().Include(lod => lod.FieldOfStudy)
                                                      .Include(lod => lod.LearningOutcomes)
                                                      .ThenInclude(lo => lo.Specialization)
                                                      .Where(lod =>
                                                                   lod.FieldOfStudy.Code == fosCode
                                                                && lod.AcademicYear == academicYear)
                                                      .FirstOrDefaultAsync();

            if (lod is null || lod.LearningOutcomes is null)
                return null;

            currentLod.LearningOutcomes = lod.LearningOutcomes;

            return await Save(currentLod);
        }

        public async Task<List<string>> History(string id)
        {
            LearningOutcomeDocument lodDb = await _dbSet.Include(lod => lod.FieldOfStudy)
                                                        .Include(lod => lod.LearningOutcomes).FirstOrDefaultAsync(l => l.Id.ToString() == id);

            List<string> versions = await _dbSet.Include(lod => lod.FieldOfStudy)
                                                .Where(lod =>
                                                             lod.AcademicYear == lod.AcademicYear
                                                          && lod.FieldOfStudy == lodDb.FieldOfStudy)
                                                .Select(lod => lod.Version).ToListAsync();
            return versions;
        }
    }
}
