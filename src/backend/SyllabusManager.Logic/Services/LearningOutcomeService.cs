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
using SyllabusManager.Data.Enums.LearningOutcomes;

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


        public async Task<bool> Pdf(Guid currentDocId)
        {
            var lod = await _dbSet.AsNoTracking()
                                  .Include(lod => lod.FieldOfStudy)
                                  .ThenInclude(fos => fos.Supervisor)
                                  .Include(lod => lod.LearningOutcomes)
                                  .ThenInclude(lo => lo.Specialization)
                                  .FirstOrDefaultAsync(l =>
                                                           l.Id == currentDocId
                                                        && l.IsDeleted == false);
            if (lod is null)
                return false;

            Data.Models.FieldOfStudies.FieldOfStudy fos = lod.FieldOfStudy;
            List<LearningOutcome> lods = lod.LearningOutcomes;
            using (Document doc = PdfHelper.Document(true))
            {
                

                doc.Add(new Paragraph("ZAKŁADANE EFEKTY UCZENIA SIĘ").SetFontSize(20));
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
                }

                if (lods != null)
                {
                    List<string> headers = new List<string>();
                    List<List<string>> cellsKnowelage = new List<List<string>>();
                    List<List<string>> cellsSkills = new List<List<string>>();
                    List<List<string>> cellsSocialCompetences = new List<List<string>>();

                    List<List<string>> specKnowelage = new List<List<string>>();
                    List<List<string>> specSkills = new List<List<string>>();
                    List<List<string>> specSocialCompetences = new List<List<string>>();

                    string spec = string.Empty;

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
                                cell.Add(EnumTranslator.Translate(prop.GetValue(l)?.ToString() ?? ""));
                            }
                        }
                        if (l.Specialization is null)
                            switch (l.Category)
                            {
                                case LearningOutcomeCategory.Knowledge:
                                    cellsKnowelage.Add(cell);
                                    break;
                                case LearningOutcomeCategory.Skills:
                                    cellsSkills.Add(cell);
                                    break;
                                case LearningOutcomeCategory.SocialCompetences:
                                    cellsSocialCompetences.Add(cell);
                                    break;
                                default:
                                    break;
                            }
                        else
                        {
                            spec = l.Specialization.Name;
                            switch (l.Category)
                            {
                                case LearningOutcomeCategory.Knowledge:
                                    specKnowelage.Add(cell);
                                    break;
                                case LearningOutcomeCategory.Skills:
                                    specSkills.Add(cell);
                                    break;
                                case LearningOutcomeCategory.SocialCompetences:
                                    specSocialCompetences.Add(cell);
                                    break;
                                default:
                                    break;
                            }

                        }


                    });

                    Table loTable = new Table(headers.Count);
                    headers.ForEach(h => loTable.AddHeaderCell(h));


                    setCells(LearningOutcomeCategory.Knowledge, cellsKnowelage, headers.Count, loTable);
                    setCells(LearningOutcomeCategory.Skills, cellsSkills, headers.Count, loTable);
                    setCells(LearningOutcomeCategory.SocialCompetences, cellsSocialCompetences, headers.Count, loTable);

                    doc.Add(loTable.SetFontSize(9));

                    Table specTable = new Table(headers.Count);
                    setCells(LearningOutcomeCategory.Knowledge, specKnowelage, headers.Count, specTable);
                    setCells(LearningOutcomeCategory.Skills, specSkills, headers.Count, specTable);
                    setCells(LearningOutcomeCategory.SocialCompetences, specSocialCompetences, headers.Count, specTable);

                    if (spec != string.Empty)
                        doc.Add(new Paragraph("Specjalność - " + spec));
                    doc.Add(specTable.SetFontSize(9));

                }
            }
            return true;
        }

        private void setCells(LearningOutcomeCategory enm, List<List<string>> items, int headers, Table tab)
        {
            if (items.Count > 0)
            {
                Cell categoryCell = new Cell(1, headers);
                categoryCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                            .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE)
                            .SetFontSize(12);

                categoryCell.Add(new Paragraph(EnumTranslator.Translate(enm.ToString())));
                tab.AddCell(categoryCell);
                items.ForEach(c =>
                {
                    foreach (var item in c)
                    {
                        tab.AddCell(new Cell().Add(new Paragraph(item)));
                    }
                });
            }
        }
    }
}
