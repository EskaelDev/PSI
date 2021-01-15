using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyllabusManager.Logic.Pdf;

namespace SyllabusManager.Logic.Services
{
    public class LearningOutcomeService : DocumentInAcademicYearService<LearningOutcomeDocument>, ILearningOutcomeService
    {
        private readonly ILearningOutcomePdf _learningOutcomePdf;

        public LearningOutcomeService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager, ILearningOutcomePdf learningOutcomePdf) : base(dbContext, userManager)
        {
            _learningOutcomePdf = learningOutcomePdf;
        }


        /// <summary>
        /// Zwraca obiekt LearningOutcomeDocument o najnowszej wersji dla podanych parametrów (jeżeli nie istnieje to zwraca nowy obiekt)
        /// </summary>
        /// <param name="fosCode">Kod FieldOfStudy(Kierunku studiów)</param>
        /// <param name="academicYear">Rok akademicki</param>
        /// <returns>LearningOutcomeDocument o najwyższej wersji</returns>
        public async Task<LearningOutcomeDocument> Latest(string fosCode, string academicYear, bool isReadOnly = false)
        {
            LearningOutcomeDocument lodDb = await _dbSet.AsNoTracking().Include(lod => lod.FieldOfStudy)
                                                        .ThenInclude(fs => fs.Specializations)
                                                        .Include(lod => lod.LearningOutcomes)
                                                        .ThenInclude(lo => lo.Specialization)
                                                        .Where(lod =>
                                                                     lod.IsDeleted == false
                                                                  && lod.AcademicYear == academicYear
                                                                  && lod.FieldOfStudy.Code == fosCode
                                                                  && !lod.IsDeleted)
                                                        .OrderByDescending(lod => lod.Version)
                                                        .FirstOrDefaultAsync();
            if (lodDb == null)
            {
                if (isReadOnly) return null;

                lodDb = new LearningOutcomeDocument
                {
                    AcademicYear = academicYear,
                    FieldOfStudy = _dbContext.FieldsOfStudies.Include(f => f.Specializations).FirstOrDefault(f => f.Code == fosCode),
                    Version = "new"
                };
            }

            if (lodDb.FieldOfStudy is null) return null;

            lodDb.FieldOfStudy?.Specializations.RemoveAll(s => s.IsDeleted);

            return lodDb;
        }

        /// <summary>
        /// Zapisuje obiekt w najnowsze wersji
        /// </summary>
        /// <param name="learningOutcome"></param>
        /// <returns>Zapisany LearningOutcomeDocument</returns>
        public async Task<LearningOutcomeDocument> Save(LearningOutcomeDocument learningOutcome)
        {
            var currentDocument = await _dbSet.AsNoTracking()
                .Include(lod => lod.FieldOfStudy)
                .ThenInclude(fs => fs.Specializations)
                .Include(lod => lod.LearningOutcomes)
                .ThenInclude(lo => lo.Specialization)
                .Where(lod =>
                    lod.IsDeleted == false
                    && lod.AcademicYear == learningOutcome.AcademicYear
                    && lod.FieldOfStudy.Code == learningOutcome.FieldOfStudy.Code
                    && !lod.IsDeleted)
                .OrderByDescending(lod => lod.Version)
                .FirstOrDefaultAsync();

            if (currentDocument is null)
            {
                learningOutcome.Version = NewVersion();
            }
            else
            {
                if (!AreChanges(currentDocument, learningOutcome)) return learningOutcome;
                learningOutcome.Version = IncreaseVersion(currentDocument.Version);
            }
            
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
        public async Task<LearningOutcomeDocument> ImportFrom(Guid currentDocId, string fosCode, string academicYear)
        {
            LearningOutcomeDocument currentLod = await _dbSet.AsNoTracking()
                                                             .Include(lod => lod.FieldOfStudy)
                                                             .Include(lod => lod.LearningOutcomes)
                                                             .FirstOrDefaultAsync(l =>
                                                                                      l.Id == currentDocId
                                                                                   && !l.IsDeleted);

            LearningOutcomeDocument lod = await _dbSet.AsNoTracking()
                                                      .Include(lod => lod.FieldOfStudy)
                                                      .Include(lod => lod.LearningOutcomes)
                                                      .ThenInclude(lo => lo.Specialization)
                                                      .Where(lod =>
                                                                   lod.FieldOfStudy.Code == fosCode
                                                                && lod.AcademicYear == academicYear
                                                                && !lod.IsDeleted)
                                                      .FirstOrDefaultAsync();

            if (currentLod is null || lod?.LearningOutcomes is null)
                return null;

            currentLod.LearningOutcomes = lod.LearningOutcomes;

            return await Save(currentLod);
        }

        /// <summary>
        /// Pobiera historię wersji (jako lista string z nazwami wersji)
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <returns></returns>
        public async Task<List<string>> History(Guid id)
        {
            LearningOutcomeDocument lodDb = await _dbSet.Include(lod => lod.FieldOfStudy)
                                                        .Include(lod => lod.LearningOutcomes).FirstOrDefaultAsync(l => l.Id == id);

            List<string> versions = await _dbSet.Include(lod => lod.FieldOfStudy)
                                                .Where(lod =>
                                                             lod.AcademicYear == lodDb.AcademicYear
                                                          && lod.FieldOfStudy == lodDb.FieldOfStudy
                                                          && !lod.IsDeleted)
                                                .OrderByDescending(l => l.Version).Select(lod => $"{lod.Id}:{lod.Version}").ToListAsync();
            return versions;
        }

        public async Task<bool> Delete(Guid id)
        {
            LearningOutcomeDocument lod = _dbSet.Include(l => l.FieldOfStudy).FirstOrDefault(f => f.Id == id);

            List<LearningOutcomeDocument> learningOutcomes = await _dbSet.Include(s => s.FieldOfStudy)
                .Where(s =>
                    s.FieldOfStudy == lod.FieldOfStudy
                    && s.AcademicYear == lod.AcademicYear
                    && !s.IsDeleted).ToListAsync();

            learningOutcomes.ForEach(s => s.IsDeleted = true);
            int state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }


        public async Task<bool> Pdf(Guid currentDocId)
        {
            LearningOutcomeDocument lod = await _dbSet.AsNoTracking()
                                  .Include(lod => lod.FieldOfStudy)
                                  .ThenInclude(fos => fos.Supervisor)
                                  .Include(lod => lod.LearningOutcomes)
                                  .ThenInclude(lo => lo.Specialization)
                                  .FirstOrDefaultAsync(l =>
                                                           l.Id == currentDocId
                                                        && l.IsDeleted == false);
            if (lod is null)
                return false;

            _learningOutcomePdf.Create(lod);

            return true;
        }

        public async Task<bool> Pdf(string fosCode, string academicYear)
        {
            LearningOutcomeDocument lod = await _dbSet.AsNoTracking()
                                  .Include(lod => lod.FieldOfStudy)
                                  .ThenInclude(fos => fos.Supervisor)
                                  .Include(lod => lod.LearningOutcomes)
                                  .ThenInclude(lo => lo.Specialization)
                                  .Where(lod =>
                                               lod.IsDeleted == false
                                            && lod.AcademicYear == academicYear
                                            && lod.FieldOfStudy.Code == fosCode
                                            && !lod.IsDeleted)
                                  .OrderByDescending(lod => lod.Version)
                                  .FirstOrDefaultAsync();
            if (lod is null)
                return false;

            _learningOutcomePdf.Create(lod);

            return true;
        }

        public static bool AreChanges(LearningOutcomeDocument previous, LearningOutcomeDocument current)
        {
            var previousJson = JsonConvert.SerializeObject(previous.LearningOutcomes.OrderBy(l => l.Symbol).ToList());
            var currentJson = JsonConvert.SerializeObject(current.LearningOutcomes.OrderBy(l => l.Symbol).ToList());
            return previousJson != currentJson;
        }
    }
}
