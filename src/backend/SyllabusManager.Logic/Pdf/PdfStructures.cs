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
    public static class PdfStructures
    {
        public static string Lorem => "Lorem ipsum dolor sit amet lorem. Praesent lacinia at, egestas at, volutpat ut, condimentum dignissim. Pellentesque nunc. Praesent gravida justo, posuere urna vitae sem. Pellentesque fringilla ligula eleifend ac, eleifend erat volutpat. Pellentesque aliquam enim. Donec ullamcorper, risus metus eleifend neque ultrices iaculis. In vitae arcu erat, molestie nulla bibendum risus. Suspendisse vel hendrerit tellus et leo. Vivamus orci sit amet, euismod eget, rutrum ligula, et netus et netus et magnis dis parturient montes, nascetur ridiculus mus. Integer mi risus, pellentesque eget, bibendum ac, molestie tincidunt. Pellentesque euismod nulla fermentum vel, arcu. Etiam vel turpis vitae lacus. Ut sed enim. ";

        public static Paragraph Info => new Paragraph("BK – liczba punktów ECTS przypisanych godzinom zajęć wymagających bezpośredniego kontaktu nauczycieli i studentów" +
                                               "\nTradycyjna – T, zdalna – Z" +
                                               "\nEgzamin -E, zaliczenie na ocenę – Z. W grupie kursów po literze E lub Z w nawiasie wpisać formę kursu końcowego (w,c,l,s,p) " +
                                               "\nKurs/grupa kursów Ogólnouczelniany – O" +
                                               "\nKurs/grupa kursów Praktyczny – P. W grupie kursów w nawiasie wpisać liczbę punktów ECTS dla kursów o charakterze praktycznym, " +
                                               "\nKO – kształcenia ogólnego, PH – podstawowy, K – kierunkowy, S – specjalnościowy" +
                                               "\nW – wybieralny, OB. – obowiązkowy").SetFontSize(7);
        public static Cell generalInfoCell(string title, string content, bool center = false)
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

        public static Table moduleTable(List<Subject> subject)
        {

            Table moduleTable = new Table(19);

            #region headers
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
            #endregion

            //w = 0, c = 1, l = 2, p = 3, s = 4, zzu = 5, cnps = 6, ects = 7, ectsbk = 8;
            int[] acc = new int[9];
            Array.Clear(acc, 0, 9);

            subject.ForEach(su =>
            {
                int i = 1;

                su.Lessons.ForEach(l =>
                {
                    moduleTable = appendToTable(i, acc, su, l, moduleTable);
                    i++;
                });
            });

            moduleTable = appendSum(moduleTable, acc);
            return moduleTable;
        }

        public static Table appendToTable(int i, int[] acc, Subject subject, Lesson lesson, Table tab)
        {
            if (subject is null)
                return tab;

            // lp
            tab.AddCell(moduleTabCell(i.ToString()));

            // kod kursu
            var sufix = EnumTranslator.Translate(lesson.LessonType.ToString()).Substring(0, 1).ToUpper();
            tab.AddCell(moduleTabCell(subject.Code + sufix + (lesson.IsGroup ? "p" : "")));

            // nazwa kursu
            tab.AddCell(moduleTabCell(subject.NamePl + (subject.Lessons.Any(l => l.IsGroup) ? "GK" : "")));

            // w
            acc[0] += lesson.LessonType == LessonType.Lecture ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Lecture ? (lesson.HoursAtUniversity/15).ToString() : ""));

            // ćw
            acc[1] += lesson.LessonType == LessonType.Classes ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Classes ? (lesson.HoursAtUniversity/15).ToString() : ""));

            // l
            acc[2] += lesson.LessonType == LessonType.Laboratory ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Laboratory ? (lesson.HoursAtUniversity/15).ToString() : ""));

            // p
            acc[3] += lesson.LessonType == LessonType.Project ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Project ? (lesson.HoursAtUniversity/15).ToString() : ""));

            // s
            acc[4] += lesson.LessonType == LessonType.Seminar ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Seminar ? (lesson.HoursAtUniversity/15).ToString() : ""));

            // symbol efektu uczenia
            tab.AddCell(moduleTabCell(subject.LearningOutcomeEvaluations.Aggregate("", (los, next) => los += next.LearningOutcomeSymbol + "\n", los => los)));

            // ZZU
            acc[5] += lesson.HoursAtUniversity;
            tab.AddCell(moduleTabCell(lesson.HoursAtUniversity.ToString()));

            //CNPS
            acc[6] += lesson.StudentWorkloadHours;
            tab.AddCell(moduleTabCell(lesson.StudentWorkloadHours.ToString()));

            //"łączna"
            acc[7] += lesson.Ects;
            tab.AddCell(moduleTabCell(lesson.Ects.ToString()));

            //"zajęć BK"
            acc[8] += lesson.EctsinclDirectTeacherStudentContactClasses;
            tab.AddCell(moduleTabCell(lesson.EctsinclDirectTeacherStudentContactClasses.ToString()));

            //"Forma kursu/grupy kursów"
            tab.AddCell(moduleTabCell("T"));

            //"Sposób zaliczenia"
            tab.AddCell(moduleTabCell(EnumTranslator.Translate(lesson.FormOfCrediting.ToString())));

            //"ogólnouczelniany"
            tab.AddCell(moduleTabCell(subject.ModuleType == ModuleType.General ? "T" : ""));

            //"o charakterze praktycznym"
            tab.AddCell(moduleTabCell(lesson.LessonType == LessonType.Project || lesson.LessonType == LessonType.Laboratory ? $"P({lesson.Ects})" : ""));

            //"rodzaj"
            tab.AddCell(moduleTabCell(EnumTranslator.Translate(subject.ModuleType.ToString())));

            //"typ"
            tab.AddCell(moduleTabCell(EnumTranslator.Translate(subject.TypeOfSubject.ToString())));


            return tab;
        }

        public static Cell moduleTabCell(string text)
        {
            return new Cell().SetFontSize(9).Add(new Paragraph(text));
        }

        public static Table appendSum(Table tab, int[] acc)
        {
            // lp
            tab.AddCell(new Cell().SetBorderBottom(Border.NO_BORDER)
                                  .SetBorderLeft(Border.NO_BORDER)
                                  .SetBorderRight(Border.NO_BORDER));

            // kod kursu
            tab.AddCell(new Cell().SetBorderBottom(Border.NO_BORDER)
                                  .SetBorderLeft(Border.NO_BORDER));

            // nazwa kursu
            tab.AddCell("Razem");

            // w
            tab.AddCell(moduleTabCell(acc[0] == 0 ? "" : acc[0].ToString()));

            // ćw
            tab.AddCell(moduleTabCell(acc[1] == 0 ? "" : acc[1].ToString()));

            // l
            tab.AddCell(moduleTabCell(acc[2] == 0 ? "" : acc[2].ToString()));

            // p
            tab.AddCell(moduleTabCell(acc[3] == 0 ? "" : acc[3].ToString()));

            // s
            tab.AddCell(moduleTabCell(acc[4] == 0 ? "" : acc[4].ToString()));

            // symbol efektu uczenia
            tab.AddCell("");

            // ZZU
            tab.AddCell(moduleTabCell(acc[5] == 0 ? "" : acc[5].ToString()));

            //CNPS
            tab.AddCell(moduleTabCell(acc[6] == 0 ? "" : acc[6].ToString()));

            //"łączna"
            tab.AddCell(moduleTabCell(acc[7] == 0 ? "" : acc[7].ToString()));

            //"zajęć BK"
            tab.AddCell(moduleTabCell(acc[8] == 0 ? "" : acc[8].ToString()));

            //"Forma kursu/grupy kursów"
            tab.AddCell("");

            //"Sposób zaliczenia"
            tab.AddCell("");

            //"ogólnouczelniany"
            tab.AddCell("");

            //"o charakterze praktycznym"
            tab.AddCell("");

            //"rodzaj"
            tab.AddCell("");

            //"typ"
            tab.AddCell("");
            return tab;
        }

        public static Table completitionCell(Table tab, SubjectInSyllabusDescription desc, int i)
        {
            if (desc.Subject == null)
                return tab;
            tab.AddCell(new Paragraph(i.ToString()));
            tab.AddCell(new Paragraph(desc.Subject.Code));
            tab.AddCell(new Paragraph(desc.Subject.NamePl));
            tab.AddCell(new Paragraph(desc.CompletionSemester.ToString()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            i++;
            return tab;
        }

        public static Table semesterSummaryTable(List<Subject> subject)
        {
            Table summaryTable = new Table(9).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
            #region headers

            summaryTable.AddCell(new Cell(1, 5).SetFontSize(9).Add(new Paragraph("Łączna liczba godzin")));
            summaryTable.AddCell(new Cell(1, 1).SetWidth(100).SetFontSize(9).Add(new Paragraph("Łączna liczba godzin ZZU")));
            summaryTable.AddCell(new Cell(1, 1).SetWidth(100).SetFontSize(9).Add(new Paragraph("Łączna liczba godzin CNPS")));
            summaryTable.AddCell(new Cell(1, 1).SetWidth(100).SetFontSize(9).Add(new Paragraph("Łączna liczba punktów ECTS")));
            summaryTable.AddCell(new Cell(1, 1).SetWidth(100).SetFontSize(9).Add(new Paragraph("Liczba punktów ECTS zajęć BK")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("w")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("ć")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("l")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("p")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("s")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("")));
            summaryTable.AddCell(new Cell(1, 1).SetFontSize(9).Add(new Paragraph("")));

            #endregion
            int[] ints = new int[9];
            Array.Clear(ints, 0, ints.Length);

            ints[0] = (subject.Sum(s => s.Lessons.Where(l => l.LessonType == LessonType.Lecture).Sum(l => l.Ects)));
            ints[1] = (subject.Sum(s => s.Lessons.Where(l => l.LessonType == LessonType.Classes).Sum(l => l.Ects)));
            ints[2] = (subject.Sum(s => s.Lessons.Where(l => l.LessonType == LessonType.Laboratory).Sum(l => l.Ects)));
            ints[3] = (subject.Sum(s => s.Lessons.Where(l => l.LessonType == LessonType.Project).Sum(l => l.Ects)));
            ints[4] = (subject.Sum(s => s.Lessons.Where(l => l.LessonType == LessonType.Seminar).Sum(l => l.Ects)));

            ints[5] = (subject.Sum(s => s.Lessons.Sum(l => l.HoursAtUniversity)));
            ints[6] = (subject.Sum(s => s.Lessons.Sum(l => l.StudentWorkloadHours)));
            ints[7] = (subject.Sum(s => s.Lessons.Sum(l => l.Ects)));
            ints[8] = (subject.Sum(s => s.Lessons.Sum(l => l.EctsinclDirectTeacherStudentContactClasses)));

            foreach (int val in ints)
            {
                summaryTable.AddCell(val.ToString());
            }

            return summaryTable;
        }

        public static Cell ParagraphCell(string text)
        {
            return new Cell().Add(new Paragraph(text));
        }


        public static void fieldOfStudyInfo(Document doc, FieldOfStudy fieldOfStudy)
        {

            // Wydział
            if (fieldOfStudy.Faculty != null)
                faculty(fieldOfStudy.Faculty, doc);

            // kierunek
            if (fieldOfStudy.Name != null)
                fieldofstudy(fieldOfStudy.Name, doc);

            // dyscyplina
            if (fieldOfStudy.Discipline != null)
                dyscipline(doc, fieldOfStudy.Discipline);

            // stopien i stacjonarnosc
            fieldOfStudyLevel(doc, fieldOfStudy.Level);

            // Forma
            fieldOfStudyType(doc, fieldOfStudy.Type);

            // Profil
            fieldOfStudyProfile(doc, fieldOfStudy.Profile);

            // Język
            fieldOfStudyLanguage(doc, fieldOfStudy.Language);
        }

        private static void fieldOfStudyLanguage(Document doc, MainLanguage Language)
        {
            doc.Add(new Paragraph("Język prowadzenia studiów: ".ToUpper())
                                    .Add(new Paragraph(EnumTranslator.Translate(Language.ToString()))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                                    .SetFixedLeading(0.5f));
        }

        private static void fieldOfStudyProfile(Document doc, StudiesProfile Profile)
        {
            doc.Add(new Paragraph("Profil: ".ToUpper())
                                    .Add(new Paragraph(EnumTranslator.Translate(Profile.ToString()))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                                    .SetFixedLeading(0.5f));
        }

        private static void fieldOfStudyType(Document doc, CourseType Type)
        {
            doc.Add(new Paragraph("Forma Studiów: ".ToUpper())
                                    .Add(new Paragraph(EnumTranslator.Translate(Type.ToString()))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                                    .SetFixedLeading(0.5f));
        }

        private static void fieldOfStudyLevel(Document doc, DegreeLevel Level)
        {
            doc.Add(new Paragraph("Poziom Kształcenia: ".ToUpper())
                                    .Add(new Paragraph(EnumTranslator.Translate(Level.ToString()))
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                                    .SetFixedLeading(0.5f));
        }

        private static void dyscipline(Document doc, string Discipline)
        {
            doc.Add(new Paragraph("Dyscyplina: ".ToUpper() + Discipline));
        }

        private static void fieldofstudy(string Name, Document doc)
        {
            doc.Add(new Paragraph("Kierunek studiów: ".ToUpper())
                                    .Add(new Paragraph(Name)
                                        .SetItalic()
                                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                                    .SetFixedLeading(0.5f));
        }

        private static void faculty(string Faculty, Document doc)
        {
            doc.Add(new Paragraph("Wydział ".ToUpper() + Faculty));
        }
    }
}
