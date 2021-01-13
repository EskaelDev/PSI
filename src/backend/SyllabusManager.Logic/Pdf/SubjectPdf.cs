using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using SyllabusManager.Data.Attributes;
using SyllabusManager.Data.Enums.FieldOfStudies;
using SyllabusManager.Data.Enums.LearningOutcomes;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace SyllabusManager.Logic.Pdf
{
    public class SubjectPdf : ISubjectPdf
    {
        #region Subject
        public void Create(Subject subject)
        {
            var fieldOfStudy = subject.FieldOfStudy;
            using (Document doc = PdfHelper.Document())
            {
                doc.SetFontSize(10);

                info(subject, fieldOfStudy, doc);

                // Tabelka
                Lesson Lecture, Classes, Laboratory, Project, Seminar;
                lessonsTable(subject, doc, out Lecture, out Classes, out Laboratory, out Project, out Seminar);

                // Wymagania, cele, narzędzia
                objectivesToolsPrerequisites(subject, doc);

                // treści programowe - forma zajęć
                programmeContent(doc, Lecture, Classes, Laboratory, Project, Seminar);

                doc.Add(new Paragraph(""));
                // OCENA OSIĄGNIĘCIA PRZEDMIOTOWYCH EFEKTÓW KSZTAŁCENIA
                achievementEvaluation(subject, doc);

                // Literatura
                literature(subject, doc);
            }
            PdfHelper.AddPages();
        }

        private static void info(Subject subject, FieldOfStudy fieldOfStudy, Document doc)
        {
            // Wydział
            subjectTitle(fieldOfStudy, doc);
            // Prowadzacy
            regularParagraph("Prowadzący: ", subject.Supervisor.Name, doc);
            // nazwa pl

            italicParagraph("Nazwa w języku polskim ", subject.NamePl, doc);
            // nazwa en
            italicParagraph("Nazwa w języku angielskim ", subject.NameEng, doc);
            // kierunek
            italicParagraph("Kierunek studiów: ", fieldOfStudy.Name, doc);
            // specjalnosc
            italicParagraph("Specjalność: ", subject.Specialization.Name, doc);

            // stopien i stacjonarnosc
            regularParagraph("Stopień studiów i forma: ", EnumTranslator.Translate(fieldOfStudy.Level.ToString())
                                                               + ", "
                                                               + EnumTranslator.Translate(fieldOfStudy.Type.ToString())
                                                               , doc);
            // rodzaj
            regularParagraph("Rodzaj przedmiotu: ", EnumTranslator.Translate(subject.TypeOfSubject.ToString()), doc);

            // kod
            regularParagraph("Kod przedmiotu: ", subject.Code.ToUpper(), doc);
            // grupa
            bool isGroup = subject.Lessons.Any(l => l.IsGroup);

            regularParagraph("Grupa kursów: ", isGroup ? "TAK" : "NIE", doc);
        }

        private static void literature(Subject subject, Document doc)
        {
            List<string> primary = subject.Literature.Where(l => l.IsPrimary).Select(l => l.Authors + ". " + l.Title + ", " + l.Distributor + " " + l.Year + ". " + l.Isbn).ToList();
            List<string> secondary = subject.Literature.Where(l => !l.IsPrimary).Select(l => l.Authors + ". " + l.Title + ", " + l.Distributor + " " + l.Year + ". " + l.Isbn).ToList();
            doc.Add(new Paragraph("LITERATURA PODSTAWOWA I UZUPEŁNIAJĄCA")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

            doc.Add(new Paragraph("LITERATURA PODSTAWOWA:")
                .SetUnderline());
            doc.Add(PdfHelper.List(primary));

            doc.Add(new Paragraph("LITERATURA UZUPEŁNIAJĄCA:")
                .SetUnderline());
            doc.Add(PdfHelper.List(secondary));
        }

        private static void achievementEvaluation(Subject subject, Document doc)
        {
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
        }

        private void programmeContent(Document doc, Lesson Lecture, Lesson Classes, Lesson Laboratory, Lesson Project, Lesson Seminar)
        {
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
        }

        private static void objectivesToolsPrerequisites(Subject subject, Document doc)
        {
            subject.CardEntries.ForEach(ce =>
            {
                doc.Add(new Paragraph(ce.Name.ToUpper())
                   .SetBold()
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                ce.Entries.OrderBy(e => e.Code).ToList().ForEach(e =>
                {
                    doc.Add(new Paragraph(e.Code + " " + e.Description));
                });

            });
        }

        private void lessonsTable(Subject subject, Document doc, out Lesson Lecture, out Lesson Classes, out Lesson Laboratory, out Lesson Project, out Lesson Seminar)
        {
            List<string> headers = lessonTableHeaders();
            List<List<string>> rows = new List<List<string>>();
            List<string> row = new List<string>();
            string[] titles = lessonTableTitles();
            row.AddRange(titles);
            rows.Add(row);
            extractLessons(subject, out Lecture, out Classes, out Laboratory, out Project, out Seminar);
            fillLessonsRows(rows, titles, Lecture, Classes, Laboratory, Project, Seminar);

            rows = rows.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                       .GroupBy(i => i.index, i => i.item)
                       .Select(g => g.ToList())
                       .ToList();

            doc.Add(PdfHelper.Table(headers, rows));
        }

        private void fillLessonsRows(List<List<string>> rows, string[] titles, Lesson Lecture, Lesson Classes, Lesson Laboratory, Lesson Project, Lesson Seminar)
        {
            rows.Add(SetLectureRow(Lecture, titles.Length));
            rows.Add(SetLectureRow(Classes, titles.Length));
            rows.Add(SetLectureRow(Laboratory, titles.Length));
            rows.Add(SetLectureRow(Project, titles.Length));
            rows.Add(SetLectureRow(Seminar, titles.Length));
        }

        private static void extractLessons(Subject subject, out Lesson Lecture, out Lesson Classes, out Lesson Laboratory, out Lesson Project, out Lesson Seminar)
        {
            Lecture = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Lecture);
            Classes = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Classes);
            Laboratory = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Laboratory);
            Project = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Project);
            Seminar = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Seminar);
        }

        private static string[] lessonTableTitles()
        {
            return new string[]
                            {
                    "Liczba godzin zajęć zorganizowanych w Uczelni (ZZU)",
                    "Liczba godzin całkowitego nakładu pracy studenta (CNPS)",
                    "Forma zaliczenia",
                    "Dla grupy kursów zaznaczyć kurs końcowy (X)",
                    "Liczba punktów ECTS",
                    "w tym liczba punktów odpowiadająca zajęciom o charakterze praktycznym (P)",
                    "w tym liczba punktów ECTS odpowiadająca zajęciom wymagającym bezpośredniego kontaktu (BK)"
                            };
        }

        private static List<string> lessonTableHeaders()
        {
            return new List<string>()
                {
                    "",
                    "Wykład",
                    "Ćwiczenia",
                    "Laboratorium",
                    "Projekt",
                    "Seminarium"
                };
        }

        private static void italicParagraph(string label, string text, Document doc)
        {
            doc.Add(new Paragraph(label)
                    .Add(new Paragraph(text)
                        .SetItalic()
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                    .SetFixedLeading(0.5f));
        }

        private static void regularParagraph(string label, string text, Document doc)
        {
            doc.Add(new Paragraph(label)
                                    .Add(new Paragraph(text)
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                                    .SetFixedLeading(0.5f));
        }

        private static void subjectTitle(FieldOfStudy fieldOfStudy, Document doc)
        {
            doc.Add(new Paragraph("Wydział ".ToUpper() + fieldOfStudy.Faculty));
            //  Karta przedmiotu
            doc.Add(new Paragraph("Karta Przedmiotu".ToUpper())
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
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

        #endregion

    }

    class CategoryTableModel
    {
        public string Name { get; set; }
        public List<List<string>> SpecKnowledge { get; set; }
        public List<List<string>> SpecSkills { get; set; }
        public List<List<string>> SpecSocialCompetences { get; set; }
        public CategoryTableModel(string name)
        {
            SpecKnowledge = new List<List<string>>();
            SpecSkills = new List<List<string>>();
            SpecSocialCompetences = new List<List<string>>();
            Name = name;
        }
        public CategoryTableModel()
        {
            SpecKnowledge = new List<List<string>>();
            SpecSkills = new List<List<string>>();
            SpecSocialCompetences = new List<List<string>>();
            Name = string.Empty;
        }
    }
}
