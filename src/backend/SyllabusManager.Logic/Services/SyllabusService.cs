using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Models.User;
using iText.Layout;
using SyllabusManager.Logic.Helpers;
using iText.Layout.Element;
using SyllabusManager.Logic.Extensions;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Enums.LearningOutcomes;

namespace SyllabusManager.Logic.Services
{
    public class SyllabusService : DocumentInAcademicYearService<Syllabus>, ISyllabusService
    {
        public SyllabusService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager) : base(dbContext, userManager)
        {

        }

        public async Task<Syllabus> Latest(string fos, string spec, string year)
        {
            Syllabus syllabus = await _dbSet.Include(s => s.FieldOfStudy)
                                       .Include(s => s.Specialization)
                                       .Include(s => s.Description)
                                       .Include(s => s.SubjectDescriptions)
                                       .ThenInclude(sd => sd.Subject)
                                       .Include(s => s.PointLimits)
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
                    Version = "new",
                    PointLimits = SyllabusHelper.PredefinedPointLimits
                };
            }

            if (syllabus.FieldOfStudy is null || syllabus.Specialization is null) return null;

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
                    Data.Models.Subjects.Subject subj = _dbContext.Subjects.Find(sd.Subject.Id);
                    sd.Subject = subj;
                }
            });

            syllabus.PointLimits.ForEach(pl => pl.Id = Guid.NewGuid());

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
            currentSyllabus.PointLimits = syllabus.PointLimits;

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
                                              .Include(s => s.PointLimits)
                                              .FirstOrDefaultAsync(s =>
                                                                       s.Id == currentDocId
                                                                    && !s.IsDeleted);

            Syllabus syllabus = await _dbSet.AsNoTracking()
                                            .Include(s => s.FieldOfStudy)
                                            .Include(s => s.Specialization)
                                            .Include(s => s.SubjectDescriptions)
                                            .ThenInclude(sd => sd.Subject)
                                            .Include(s => s.Description)
                                            .Include(s => s.PointLimits)
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
            currentSyllabus.PointLimits = syllabus.PointLimits;

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
                                                .OrderByDescending(s => s.Version)
                                                .Select(s => $"{s.Id}:{s.Version}")
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

        public async Task<bool> Pdf(Guid id)
        {
            Syllabus syllabus = await _dbSet.Include(s => s.FieldOfStudy)
                                            .Include(s => s.Specialization)
                                            .Include(s => s.SubjectDescriptions)
                                            .ThenInclude(sd => sd.Subject)
                                            .ThenInclude(sb=>sb.Lessons)
                                            .Include(s => s.Description)
                                            .Include(s => s.PointLimits)
                                            .FirstOrDefaultAsync(s =>
                                                                     s.Id == id
                                                                  && !s.IsDeleted);



            using (Document doc = PdfHelper.Document(true))
            {
                const string lorem = "Lorem ipsum dolor sit amet lorem. Praesent lacinia at, egestas at, volutpat ut, condimentum dignissim. Pellentesque nunc. Praesent gravida justo, posuere urna vitae sem. Pellentesque fringilla ligula eleifend ac, eleifend erat volutpat. Pellentesque aliquam enim. Donec ullamcorper, risus metus eleifend neque ultrices iaculis. In vitae arcu erat, molestie nulla bibendum risus. Suspendisse vel hendrerit tellus et leo. Vivamus orci sit amet, euismod eget, rutrum ligula, et netus et netus et magnis dis parturient montes, nascetur ridiculus mus. Integer mi risus, pellentesque eget, bibendum ac, molestie tincidunt. Pellentesque euismod nulla fermentum vel, arcu. Etiam vel turpis vitae lacus. Ut sed enim. ";

                doc.Add(new Paragraph("Program Studiów".ToUpper())
                                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                doc.SetFontSize(12);

                // Wydział
                doc.Add(new Paragraph("Wydział ".ToUpper() + syllabus.FieldOfStudy.Faculty));

                // kierunek
                doc.Add(new Paragraph("Kierunek studiów: ".ToUpper())
                        .Add(new Paragraph(syllabus.FieldOfStudy.Name)
                            .SetItalic()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));

                // dyscyplina
                doc.Add(new Paragraph("Dyscyplina: ".ToUpper() + syllabus.FieldOfStudy.Discipline));

                // stopien i stacjonarnosc
                doc.Add(new Paragraph("Poziom Kształcenia: ".ToUpper())
                        .Add(new Paragraph(EnumTranslator.Translate(syllabus.FieldOfStudy.Level.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));

                // Forma
                doc.Add(new Paragraph("Forma Studiów: ".ToUpper())
                        .Add(new Paragraph(EnumTranslator.Translate(syllabus.FieldOfStudy.Type.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // Profil
                doc.Add(new Paragraph("Profil: ".ToUpper())
                        .Add(new Paragraph(EnumTranslator.Translate(syllabus.FieldOfStudy.Profile.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));

                // Język
                doc.Add(new Paragraph("Język prowadzenia studiów: ".ToUpper())
                        .Add(new Paragraph(EnumTranslator.Translate(syllabus.FieldOfStudy.Language.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));

                // tabelka opis programu studiów
                doc.Add(new Paragraph("1. Opis ogólny").SetBold());
                Table generalInfoTable = new Table(2);

                generalInfoTable.AddCell(generalInfoCell("1.1 Liczba semestrów:",
                    syllabus.Description.NumOfSemesters.ToString(),
                    true));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.2 Całkowita liczba punktów ECTS konieczna do ukończenia studiów na danym poziomie:",
                        syllabus.Description.Ects.ToString(),
                        true));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.3 Łączna liczba godzin zajęć:",
                        "1020", // TODO: znaleźć pole od tego
                        true));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.4 Wymagania wstępne:",
                        syllabus.Description.Prerequisites));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.5 Tytuł zawodowy nadawany po zakończeniu studiów:",
                        EnumTranslator.Translate(syllabus.Description.ProfessionalTitleAfterGraduation.ToString()),
                        true));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.6 Sylwetka absolwenta, możliwości zatrudnienia:",
                        syllabus.Description.EmploymentOpportunities));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.7 Możliwość kontynuacji studiów:",
                        syllabus.Description.PossibilityOfContinuation));

                generalInfoTable.AddCell(
                    generalInfoCell(
                        "1.8 Wskazanie związku z misją uczelni i strategią jej rozwoju:",
                        "Brak")); // ToDo: coś z tym zrobić, usunąć albo nie wiem

                doc.Add(generalInfoTable);


                // Opis szeczgółowy
                var lods = (await _dbContext.LearningOutcomeDocuments.Include(lod => lod.FieldOfStudy)
                                                                     .Include(lod => lod.LearningOutcomes)
                                                                     .OrderByDescending(lod => lod.Version)
                                                                     .FirstOrDefaultAsync(lod =>
                                                                                                lod.FieldOfStudy.Code == syllabus.FieldOfStudy.Code
                                                                                             && lod.AcademicYear == syllabus.AcademicYear
                                                                                             && !lod.IsDeleted)
                                                                     )?.LearningOutcomes?.GroupBy(l => l.Category)
                                                                                         .Select(c => new { category = c.Key, count = c.Count() })
                                                                                         .ToDictionary(d => d.category);

                var w = lods[LearningOutcomeCategory.Knowledge].count;
                var u = lods[LearningOutcomeCategory.Skills].count;
                var k = lods[LearningOutcomeCategory.SocialCompetences].count;

                doc.Add(new Paragraph("2.   Opis szczegółowy"));

                doc.Add(new Paragraph($"2.1   Całkowita liczba efektów uczenia się w programie studiów: W(wiedza)= {w}, U (umiejętności) = {u}, K (kompetencje) = {k}, W + U + K = {w + u + k}"));

                doc.Add(new Paragraph($"2.2   Dla kierunku studiów przyporządkowanego do więcej niż jednej dyscypliny - liczba efektów uczenia się przypisana do dyscypliny \n\t D1(wiodąca){syllabus.FieldOfStudy.Discipline}: {w + u + k}"));

                doc.Add(new Paragraph($"2.3   Dla kierunku studiów przyporządkowanego do więcej niż jednej dyscypliny - procentowy udział liczby punktów ECTS dla każdej z dyscyplin: \n\t D1 100% punktów ECTS"));

                var suma = syllabus.SubjectDescriptions.Select(s => new { kind = s.Subject.ModuleType, ects = s.Subject.Lessons.Sum(l => l.Ects) })
                                                       .Where(s => s.kind == ModuleType.General) // ToDO: Możliwe że trzeba dodać typy
                                                       .Sum(s => s.ects);
                doc.Add(new Paragraph($"2.4a   Dla kierunku studiów o profilu ogólnoakademickim - liczba punktów ECTS przypisana zajęciom związanym z prowadzoną na Uczelni działalnością naukową w dyscyplinie lub dyscyplinach, do których przeyporządkowany jest kierunek studiów")
                                            .Add(new Paragraph("musi być większa niż 50% całkowitej liczby punktów ECTS z p 1.1  ").SetFontSize(10))
                                            .Add(new Paragraph(suma.ToString())));


                doc.Add(new Paragraph("2.5   Zwięzła analiza zgodności zakładanych efektów uczenia się z potrzebami rynku pracy"));

                doc.Add(new Paragraph(lorem));

                int directEcts = syllabus.SubjectDescriptions.Sum(sd => sd.Subject.Lessons.Sum(l => l.EctsinclDirectTeacherStudentContactClasses));
                doc.Add(new Paragraph("2.6   Łączna liczba punktów ECTS, którą student musi uzyskać na zajęciach wymagających bezpośredniego udziału nauczycieli akademickich lub innych osób prowadzących zajęcia i studentów (wpisać sumę punktów ECTS dla kursów/grup kursów oznaczonych kodem BK)")
                    .Add(new Paragraph(directEcts.ToString() + " Punkty ECTS")));

                doc.Add(new Paragraph("2.7   Łączna liczba punktów ECTS, którą student musi uzyskać w ramach zajęć z zakresu nauk podstawowych"));

                var obligatoryBasicScienceEcts = syllabus.SubjectDescriptions.Where(sd => sd.Subject.ModuleType == ModuleType.BasicScience
                                                                                       && sd.Subject.TypeOfSubject == TypeOfSubject.Obligatory)
                                                                             .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));

                var electiveBasicScienceEcts = syllabus.SubjectDescriptions.Where(sd => sd.Subject.ModuleType == ModuleType.BasicScience
                                                                                     && sd.Subject.TypeOfSubject == TypeOfSubject.Elective)
                                                                           .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));
                doc.Add(new Paragraph($"Liczba punktów ECTS z przedmiotów wybieralnych:   {obligatoryBasicScienceEcts}"));
                doc.Add(new Paragraph($"Liczba punktów ECTS z przedmiotów obowiązkowych:   {electiveBasicScienceEcts}"));
                doc.Add(new Paragraph($"Łączna liczba punktów ECTS:   {obligatoryBasicScienceEcts + electiveBasicScienceEcts}"));
                //var t = Newtonsoft.Json.JsonConvert.SerializeObject(syllabus);

                int minGeneralEcts = syllabus.PointLimits.FirstOrDefault(pl => pl.ModuleType == ModuleType.General).Points;
                doc.Add(new Paragraph("2.9   Minimalna liczba punktów ECTS, którą student musi uzyskać, realizując bloki kształcenia oferowane na zajęciach ogólnouczelnianych lub na innym kierunku studiów (wpisać sumę ECTS kursów/grup kursów oznaczonych kodem O) ")
                                    .Add(new Paragraph($"{minGeneralEcts} ECTS")));

                int totalElectiveEcts = syllabus.SubjectDescriptions.Where(sd => sd.Subject.TypeOfSubject == TypeOfSubject.Elective)
                                                                    .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));
                doc.Add(new Paragraph("2.10   Łączna liczba punktów ECTS, którą student może uzyskać, realizując bloki wybieralne (min. 30% całkowitej liczby punktów ECTS) ")
                                    .Add(new Paragraph($"{totalElectiveEcts} ECTS")));

                doc.Add(new Paragraph("3.   Opis Procesu prowadzącego do uzyskania efektów uczenia się:"));
                doc.Add(new Paragraph(lorem));


                // lista modółów
                doc.Add(new Paragraph("Lista modułów kształcenia").SetFontSize(14));
                doc.Add(new Paragraph("4.1.   Lista modułów obowiązkowych").SetFontSize(14));
                doc.Add(new Paragraph("4.1.1.   Lista modułów kształcenia ogólnego").SetFontSize(13));
                
                // humanistyczne
                doc.Add(new Paragraph("4.1.1.1.   Moduł ").SetFontSize(13).Add(new Paragraph("przedmioty z obszaru nauk humanistycznych").SetItalic().SetFontSize(13)));
                doc.Add(ModuleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Maths).Select(s => s.Subject).ToList()));

                // ToDo: Dopisać reszte tabel


            }

            if (syllabus is null)
                return false;
            return true;
        }

        private Cell generalInfoCell(string title, string content, bool center = false)
        {
            Cell cell = new Cell();
            cell.Add(new Paragraph(title).SetItalic());
            if (center)
                cell.Add(new Paragraph(content)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            else
                cell.Add(new Paragraph(content));
            return cell;
        }

        private Table ModuleTable(List<Subject> subject)
        {

            Table moduleTable = new Table(19);

            moduleTable.AddCell(new Cell(2, 1).SetFontSize(9).Add(new Paragraph("L.p")));
            moduleTable.AddCell(new Cell(2, 1).SetFontSize(9).Add(new Paragraph("Kod kursu/grupy kursów")));
            moduleTable.AddCell(new Cell(2, 1).SetFontSize(9).Add(new Paragraph("Nazwa kursu/grupy kursów (grupę kursów oznaczyć symbolem GK)")));
            moduleTable.AddCell(new Cell(1, 5).SetFontSize(9).Add(new Paragraph("Tygodniowa liczba godzin")));
            moduleTable.AddCell(new Cell(2, 1).SetFontSize(9).Add(new Paragraph("Symbol kierunk. efektu uczenia się")));
            moduleTable.AddCell(new Cell(1, 2).SetFontSize(9).Add(new Paragraph("Liczba godzin")));
            moduleTable.AddCell(new Cell(1, 2).SetFontSize(9).Add(new Paragraph("Liczba ptk. ECTS")));
            moduleTable.AddCell(new Cell(2, 1).SetFontSize(9).Add(new Paragraph("Forma kursu/grupy kursów")));
            moduleTable.AddCell(new Cell(2, 1).SetFontSize(9).Add(new Paragraph("Sposób zaliczenia")));
            moduleTable.AddCell(new Cell(1, 4).SetFontSize(9).Add(new Paragraph("Kurs/Grupa kursów")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("w")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("ć")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("l")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("p")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("s")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("ZZU")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("CNPS")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("łączna")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("zajęć BK")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("ogólnouczelniany")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("o charakterze praktycznym")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("rodzaj")));
            moduleTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("typ")));

            subject.ForEach(s =>
            {
                int i = 1;
                s.Lessons.ForEach(l =>
                {
                    
                    moduleTable = AppendToTable(i, s, l, moduleTable);
                    i++;
                });

            });
            return moduleTable;
        }

        private Table AppendToTable(int i, Subject subject, Lesson lesson, Table tab)
        {
            if (subject is null)
                return tab;
            // lp
            tab.AddCell(ModuleTabCell(i.ToString()));
            // kod kursu
            tab.AddCell(ModuleTabCell(subject.Code));
            // nazwa kursu
            tab.AddCell(ModuleTabCell(subject.NamePl + (subject.Lessons.Any(l => l.IsGroup) ? "GK" : "")));
            // w
            tab.AddCell(ModuleTabCell(
                lesson.LessonType == LessonType.Lecture ? lesson.HoursAtUniversity.ToString() : ""));
            // ćw
            tab.AddCell(ModuleTabCell(
                lesson.LessonType == LessonType.Classes ? lesson.HoursAtUniversity.ToString() : ""));
            // l
            tab.AddCell(ModuleTabCell(
                lesson.LessonType == LessonType.Laboratory ? lesson.HoursAtUniversity.ToString() : ""));
            // p
            tab.AddCell(ModuleTabCell(
                lesson.LessonType == LessonType.Project ? lesson.HoursAtUniversity.ToString() : ""));
            // s
            tab.AddCell(ModuleTabCell(
                lesson.LessonType == LessonType.Seminar ? lesson.HoursAtUniversity.ToString() : ""));
            // symbol efektu uczenia

            tab.AddCell(ModuleTabCell(subject.LearningOutcomeEvaluations.Aggregate("", (los, next) => los += next.LearningOutcomeSymbol + "\n", los => los)));
            // ZZU
            tab.AddCell(ModuleTabCell(lesson.HoursAtUniversity.ToString()));
            //CNPS
            tab.AddCell(ModuleTabCell(lesson.StudentWorkloadHours.ToString()));
            //"łączna"
            tab.AddCell(ModuleTabCell(lesson.Ects.ToString()));
            //"zajęć BK"
            tab.AddCell(ModuleTabCell(lesson.EctsinclDirectTeacherStudentContactClasses.ToString()));
            //"Forma kursu/grupy kursów"
            tab.AddCell(ModuleTabCell("T"));
            //"Sposób zaliczenia"
            tab.AddCell(ModuleTabCell(EnumTranslator.Translate(lesson.FormOfCrediting.ToString())));
            //"ogólnouczelniany"
            tab.AddCell(ModuleTabCell(subject.ModuleType == ModuleType.General ? "T" : ""));
            //"o charakterze praktycznym"
            tab.AddCell(ModuleTabCell(lesson.LessonType == LessonType.Project || lesson.LessonType == LessonType.Laboratory ? $"P({lesson.Ects})" : ""));
            //"rodzaj"
            tab.AddCell(ModuleTabCell(EnumTranslator.Translate(subject.ModuleType.ToString())));
            //"typ"
            tab.AddCell(ModuleTabCell(EnumTranslator.Translate(subject.TypeOfSubject.ToString())));


            return tab;
        }

        private Cell ModuleTabCell(string text)
        {
            return new Cell().SetFontSize(9).Add(new Paragraph(text));
        }
    }
}

