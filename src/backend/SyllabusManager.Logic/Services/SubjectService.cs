using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.ManyToMany;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using iText.Layout;
using iText.Layout.Element;
using SyllabusManager.Logic.Extensions;
using iText.Layout.Borders;

namespace SyllabusManager.Logic.Services
{
    public class SubjectService : DocumentInAcademicYearService<Subject>, ISubjectService
    {
        public SubjectService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager) : base(dbContext, userManager)
        {

        }

        public async Task<List<Subject>> GetAll(string fos, string spec, string year, SyllabusManagerUser user)
        {
            var latestIds = _dbSet.AsNoTracking().Where(s =>
                s.FieldOfStudy.Code == fos
                && s.Specialization.Code == spec
                && s.AcademicYear == year
                && !s.IsDeleted).ToList()
                .GroupBy(s => new { s.FieldOfStudy, s.Specialization, s.AcademicYear })
                .Select(g => g.OrderByDescending(s => s.Version)
                    .First().Id);

            var result = await _dbSet.Include(s => s.FieldOfStudy)
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
                var existing = await _dbSet.Include(s => s.FieldOfStudy)
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
                                                .OrderByDescending(s => s.Version)
                                                .Select(s => $"{s.Id}:{s.Version}")
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

        public string GetSupervisorId(Guid documentId)
        {
            return _dbSet.Include(s => s.Supervisor)
                .FirstOrDefault(s => s.Id == documentId)?.Supervisor?.Id;
        }

        public async Task<bool> Pdf(Guid id)
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
                                                               s.Id == id
                                                            && !s.IsDeleted);
            if (subject is null)
                return false;


            using (Document doc = PdfHelper.Document())
            {
                doc.SetFontSize(10);

                // Wydział
                doc.Add(new Paragraph("Wydział ".ToUpper() + subject.FieldOfStudy.Faculty));
                //  Karta przedmiotu
                doc.Add(new Paragraph("Karta Przedmiotu".ToUpper())
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                // Prowadzacy
                doc.Add(new Paragraph("Prowadzący: ")
                        .Add(new Paragraph(subject.Supervisor.Name)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // nazwa pl
                doc.Add(new Paragraph("Nazwa w języku polskim ")
                        .Add(new Paragraph(subject.NamePl)
                            .SetItalic()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // nazwa en
                doc.Add(new Paragraph("Nazwa w języku angielskim ")
                        .Add(new Paragraph(subject.NameEng)
                            .SetItalic()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // kierunek
                doc.Add(new Paragraph("Kierunek studiów: ")
                        .Add(new Paragraph(subject.FieldOfStudy.Name)
                            .SetItalic()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // specjalnosc
                doc.Add(new Paragraph("Specjalność: ")
                        .Add(new Paragraph(subject.Specialization.Name)
                            .SetItalic()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // stopien i stacjonarnosc
                doc.Add(new Paragraph("Stopień studiów i forma: ")
                        .Add(new Paragraph(
                                           EnumTranslator.Translate(subject.FieldOfStudy.Level.ToString())
                                         + ", "
                                         + EnumTranslator.Translate(subject.FieldOfStudy.Type.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // rodzaj
                doc.Add(new Paragraph("Rodzaj przedmiotu: ")
                        .Add(new Paragraph(EnumTranslator.Translate(subject.TypeOfSubject.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // kod
                doc.Add(new Paragraph("Kod przedmiotu ")
                        .Add(new Paragraph(subject.Code.ToUpper())
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // grupa
                var isGroup = subject.Lessons.Any(l => l.IsGroup);

                doc.Add(new Paragraph("Grupa kursów ")
                        .Add(new Paragraph(isGroup ? "TAK" : "NIE")
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));


                // Tabelka
                List<string> headers = new List<string>()
                {
                    "",
                    "Wykład",
                    "Ćwiczenia",
                    "Laboratorium",
                    "Projekt",
                    "Seminarium"
                };
                List<List<string>> rows = new List<List<string>>();
                List<string> row = new List<string>();
                string[] titles = new string[]
                {
                    "Liczba godzin zajęć zorganizowanych w Uczelni (ZZU)",
                    "Liczba godzin całkowitego nakładu pracy studenta (CNPS)",
                    "Forma zaliczenia",
                    "Dla grupy kursów zaznaczyć kurs końcowy (X)",
                    "Liczba punktów ECTS",
                    "w tym liczba punktów odpowiadająca zajęciom o charakterze praktycznym (P)",
                    "w tym liczba punktów ECTS odpowiadająca zajęciom wymagającym bezpośredniego kontaktu (BK)"
                };
                row.AddRange(titles);
                rows.Add(row);

                var Lecture = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Lecture);
                var Classes = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Classes);
                var Laboratory = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Laboratory);
                var Project = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Project);
                var Seminar = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Seminar);

                rows.Add(SetLectureRow(Lecture, titles.Length));
                rows.Add(SetLectureRow(Classes, titles.Length));
                rows.Add(SetLectureRow(Laboratory, titles.Length));
                rows.Add(SetLectureRow(Project, titles.Length));
                rows.Add(SetLectureRow(Seminar, titles.Length));

                rows = rows.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                           .GroupBy(i => i.index, i => i.item)
                           .Select(g => g.ToList())
                           .ToList();

                doc.Add(PdfHelper.Table(headers, rows));

                // Wymagania, cele, narzędzia
                subject.CardEntries.ForEach(ce =>
                {
                    doc.Add(new Paragraph(ce.Name.ToUpper())
                       .SetBold()
                       .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                    ;
                    ce.Entries.OrderBy(e => e.Code).ToList().ForEach(e =>
                    {
                        doc.Add(new Paragraph(e.Code + " " + e.Description));
                    });

                });

                // treści programowe - forma zajęć
                Table classFormsTable = new Table(3);
                classFormsTable.SetWidth(500);
                Cell h1 = new Cell(1, 3).Add(new Paragraph("TREŚCI PROGRAMOWE")).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                classFormsTable.AddCell(h1);

                classFormsTable = SetClassFormsRow("Wykład", classFormsTable, Lecture);
                classFormsTable = SetClassFormsRow("Ćwiczenia", classFormsTable, Classes);
                classFormsTable = SetClassFormsRow("Laboratoria", classFormsTable, Laboratory);
                classFormsTable = SetClassFormsRow("Projekt", classFormsTable, Project);
                classFormsTable = SetClassFormsRow("Seminarium", classFormsTable, Seminar);


                doc.Add(classFormsTable);


                doc.Add(new Paragraph(""));
                // OCENA OSIĄGNIĘCIA PRZEDMIOTOWYCH EFEKTÓW KSZTAŁCENIA
                doc.Add(new Paragraph("OCENA OSIĄGNIĘCIA PRZEDMIOTOWYCH EFEKTÓW KSZTAŁCENIA")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                Table learningOutcomeTable = new Table(3);
                learningOutcomeTable.AddCell("Oceny (F – formująca (w trakcie semestru), P – podsumowująca(na koniec semestru)");
                learningOutcomeTable.AddCell("Numer efektu kształcenia");
                learningOutcomeTable.AddCell("Sposób oceny osiągnięcia efektu kształcenia");

                subject.LearningOutcomeEvaluations.ForEach(loe =>
                {
                    learningOutcomeTable.AddCell(EnumTranslator.Translate(loe.GradingSystem.ToString()));
                    learningOutcomeTable.AddCell(loe.LearningOutcomeSymbol);
                    learningOutcomeTable.AddCell(loe.Description);
                });
                doc.Add(learningOutcomeTable);

                // Literatura
                doc.Add(new Paragraph("LITERATURA PODSTAWOWA I UZUPEŁNIAJĄCA")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                var primary = subject.Literature.Where(l => l.IsPrimary).Select(l => l.Authors + ". " + l.Title + ", " + l.Distributor + " " + l.Year + ". " + l.Isbn).ToList();
                var secondary = subject.Literature.Where(l => !l.IsPrimary).Select(l => l.Authors + ". " + l.Title + ", " + l.Distributor + " " + l.Year + ". " + l.Isbn).ToList();
                
                doc.Add(new Paragraph("LITERATURA PODSTAWOWA:")
                    .SetUnderline());
                doc.Add(PdfHelper.List(primary));

                doc.Add(new Paragraph("LITERATURA UZUPEŁNIAJĄCA:")
                    .SetUnderline());
                doc.Add(PdfHelper.List(secondary));
            }

            return true;
        }

        private List<string> SetLectureRow(Lesson lesson, int headerCount)
        {
            List<string> row = new List<string>();

            if (lesson is null)
            {
                for (int i = 0; i < headerCount; i++)
                {
                    row.Add("");
                }
            }
            else
            {
                row.Add(lesson.HoursAtUniversity.ToString());
                row.Add(lesson.StudentWorkloadHours.ToString());
                row.Add(EnumTranslator.Translate(lesson.FormOfCrediting.ToString()));
                row.Add(lesson.IsGroup ? "X" : "");
                row.Add(lesson.Ects.ToString());
                row.Add(lesson.EctsinclPracticalClasses.ToString());
                row.Add(lesson.EctsinclDirectTeacherStudentContactClasses.ToString());
            }

            return row;
        }

        private Table SetClassFormsRow(string type, Table table, Lesson lesson)
        {
            if (lesson == null)
                return table;
            Cell h2 = new Cell(1, 2).Add(new Paragraph($"Forma zajęć - {type}"));
            h2.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
            Cell h3 = new Cell().Add(new Paragraph("Liczba godzin"));
            h3.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
            table.AddCell(h2);
            table.AddCell(h3);
            string prefix = type.Substring(0, 2);
            int i = 1;
            int sum = 0;
            lesson.ClassForms.ForEach(cf =>
            {
                table.AddCell(prefix + i.ToString());
                table.AddCell(cf.Description);
                table.AddCell(new Cell().Add(new Paragraph(cf.Hours.ToString())).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                sum += cf.Hours;
                i++;
            });
            table.AddCell("");
            table.AddCell("Suma godzin");
            table.AddCell(new Cell().Add(new Paragraph(sum.ToString())).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            return table;
        }

    }
}
