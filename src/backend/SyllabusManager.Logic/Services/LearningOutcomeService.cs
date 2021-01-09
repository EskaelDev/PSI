using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using iText.Layout;
using iText.Layout.Element;
using System.Reflection;
using SyllabusManager.Data.Attributes;
using SyllabusManager.Data.Models.FieldOfStudies;

namespace SyllabusManager.Logic.Services
{
    public class LearningOutcomeService : DocumentInAcademicYearService<LearningOutcomeDocument>, ILearningOutcomeService
    {
        public LearningOutcomeService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager) : base(dbContext, userManager)
        {

        }


        /// <summary>
        /// Zwraca obiekt LearningOutcomeDocument o najnowszej wersji dla podanych parametrów (jeżeli nie istnieje to zwraca nowy obiekt)
        /// </summary>
        /// <param name="fosCode">Kod FieldOfStudy(Kierunku studiów)</param>
        /// <param name="academicYear">Rok akademicki</param>
        /// <returns>LearningOutcomeDocument o najwyższej wersji</returns>
        public async Task<LearningOutcomeDocument> Latest(string fosCode, string academicYear, bool isReadOnly = false)
        {
            LearningOutcomeDocument lodDb = await _dbSet.Include(lod => lod.FieldOfStudy)
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
                                                .Select(lod => lod.Version).OrderBy(l => l).ToListAsync();
            return versions;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = _dbSet.Include(lod => lod.FieldOfStudy).FirstOrDefault(f => f.Id == id);

            var learningOutcomes = await _dbSet.Include(s => s.FieldOfStudy)
                .Where(s =>
                    s.FieldOfStudy == entity.FieldOfStudy
                    && s.AcademicYear == entity.AcademicYear
                    && !s.IsDeleted).ToListAsync();

            learningOutcomes.ForEach(s => s.IsDeleted = true);
            var state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }


        public async Task<bool> Pdf(Guid currentDocId, string version)
        {
            var lod = await _dbSet.AsNoTracking()
                                  .Include(lod => lod.FieldOfStudy)
                                  .ThenInclude(fos => fos.Supervisor)
                                  .Include(lod => lod.LearningOutcomes)
                                  .ThenInclude(lo => lo.Specialization)
                                  .FirstOrDefaultAsync(l =>
                                      (version == null ? l.Id == currentDocId : l.Version == version)
                                      && l.IsDeleted == false);
            if (lod is null)
                return false;

            Data.Models.FieldOfStudies.FieldOfStudy fos = lod.FieldOfStudy;
            List<LearningOutcome> lods = lod.LearningOutcomes;
            using (Document doc = PdfHelper.Document())
            {
                doc.SetFont(PdfHelper.FONT);

                doc.Add(new Paragraph("ZAKŁADANE EFEKTY UCZENIA SIĘ"));
                doc.Add(new Paragraph($"Rok akademicki: {lod.AcademicYear}"));
                doc.Add(new Paragraph($"Kierunek"));

                if (fos != null)
                {
                    foreach (PropertyInfo prop in typeof(FieldOfStudy).GetProperties())
                    {
                        if (Attribute.IsDefined(prop, typeof(PdfNameAttribute)))
                        {
                            var propName = ((PdfNameAttribute)prop.GetCustomAttribute(typeof(PdfNameAttribute), true)).name;
                            var value = prop.GetValue(fos)?.ToString() ?? "";

                            doc.Add(new Paragraph(EnumTranslator.Translate(propName) + " - " + EnumTranslator.Translate(value)));
                        }
                    }
                    //doc.Add(new Paragraph($"Nazwa: {fos.Name}"));
                    //doc.Add(new Paragraph($"Kod: {fos.Code}"));
                    //doc.Add(new Paragraph($"Poziom: {fos.Level}"));
                    //doc.Add(new Paragraph($"Profil: {fos.Profile}"));
                    //doc.Add(new Paragraph($"Gałąź nauki: {fos.BranchOfScience}"));
                    //doc.Add(new Paragraph($"Dziedzina: {fos.Discipline}"));
                    //doc.Add(new Paragraph($"Wydział: {fos.Faculty}"));
                    //doc.Add(new Paragraph($"Typ kursu: {fos.Type}"));
                    //doc.Add(new Paragraph($"Język główny: {fos.Language}"));

                }

                if (lods != null)
                {
                    List<string> headers = new List<string>();
                    List<List<string>> cells = new List<List<string>>();


                    foreach (PropertyInfo prop in typeof(LearningOutcome).GetProperties())
                    {
                        if (Attribute.IsDefined(prop, typeof(PdfNameAttribute)))
                        {
                            var propName = ((PdfNameAttribute)prop.GetCustomAttribute(typeof(PdfNameAttribute), false)).name;
                            headers.Add(propName);
                        }
                    }



                    lods.ForEach(l =>
                    {
                        List<string> cell = new List<string>();
                        foreach (PropertyInfo prop in typeof(LearningOutcome).GetProperties())
                        {
                            if (Attribute.IsDefined(prop, typeof(PdfNameAttribute)))
                            {
                                cell.Add(prop.GetValue(l)?.ToString() ?? "");
                            }
                        }
                        cells.Add(cell);
                    });

                    doc.Add(PdfHelper.Table(headers, cells));
                }


            }
            return true;
        }
    }
}
