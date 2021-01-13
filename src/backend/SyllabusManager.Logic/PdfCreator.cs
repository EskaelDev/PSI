using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using SyllabusManager.Data.Attributes;
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

namespace SyllabusManager.Logic
{
    public class PdfCreator
    {

        private const string lorem = "Lorem ipsum dolor sit amet lorem. Praesent lacinia at, egestas at, volutpat ut, condimentum dignissim. Pellentesque nunc. Praesent gravida justo, posuere urna vitae sem. Pellentesque fringilla ligula eleifend ac, eleifend erat volutpat. Pellentesque aliquam enim. Donec ullamcorper, risus metus eleifend neque ultrices iaculis. In vitae arcu erat, molestie nulla bibendum risus. Suspendisse vel hendrerit tellus et leo. Vivamus orci sit amet, euismod eget, rutrum ligula, et netus et netus et magnis dis parturient montes, nascetur ridiculus mus. Integer mi risus, pellentesque eget, bibendum ac, molestie tincidunt. Pellentesque euismod nulla fermentum vel, arcu. Etiam vel turpis vitae lacus. Ut sed enim. ";

        private Paragraph Info => new Paragraph("BK – liczba punktów ECTS przypisanych godzinom zajęć wymagających bezpośredniego kontaktu nauczycieli i studentów" +
                                               "\nTradycyjna – T, zdalna – Z" +
                                               "\nEgzamin -E, zaliczenie na ocenę – Z. W grupie kursów po literze E lub Z w nawiasie wpisać formę kursu końcowego (w,c,l,s,p) " +
                                               "\nKurs/grupa kursów Ogólnouczelniany – O" +
                                               "\nKurs/grupa kursów Praktyczny – P. W grupie kursów w nawiasie wpisać liczbę punktów ECTS dla kursów o charakterze praktycznym, " +
                                               "\nKO – kształcenia ogólnego, PH – podstawowy, K – kierunkowy, S – specjalnościowy" +
                                               "\nW – wybieralny, OB. – obowiązkowy").SetFontSize(7);


        #region Syllabus & Plan
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

        private Table moduleTable(List<Subject> subject)
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

        private Table appendToTable(int i, int[] acc, Subject subject, Lesson lesson, Table tab)
        {
            if (subject is null)
                return tab;

            // lp
            tab.AddCell(moduleTabCell(i.ToString()));

            // kod kursu
            tab.AddCell(moduleTabCell(subject.Code));

            // nazwa kursu
            tab.AddCell(moduleTabCell(subject.NamePl + (subject.Lessons.Any(l => l.IsGroup) ? "GK" : "")));

            // w
            acc[0] += lesson.LessonType == LessonType.Lecture ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Lecture ? lesson.HoursAtUniversity.ToString() : ""));

            // ćw
            acc[1] += lesson.LessonType == LessonType.Classes ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Classes ? lesson.HoursAtUniversity.ToString() : ""));

            // l
            acc[2] += lesson.LessonType == LessonType.Laboratory ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Laboratory ? lesson.HoursAtUniversity.ToString() : ""));

            // p
            acc[3] += lesson.LessonType == LessonType.Project ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Project ? lesson.HoursAtUniversity.ToString() : ""));

            // s
            acc[4] += lesson.LessonType == LessonType.Seminar ? lesson.HoursAtUniversity : 0;
            tab.AddCell(moduleTabCell(
                lesson.LessonType == LessonType.Seminar ? lesson.HoursAtUniversity.ToString() : ""));

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

        private Cell moduleTabCell(string text)
        {
            return new Cell().SetFontSize(9).Add(new Paragraph(text));
        }

        private Table appendSum(Table tab, int[] acc)
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

        private Table completitionCell(Table tab, SubjectInSyllabusDescription desc, int i)
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

        private Table semesterSummaryTable(List<Subject> subject)
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

        private Cell ParagraphCell(string text)
        {
            return new Cell().Add(new Paragraph(text));
        }

        #endregion
        public void Create(Syllabus syllabus, Dictionary<LearningOutcomeCategory, int> lods)
        {
            using (Document doc = PdfHelper.Document(true))
            {
                #region setup i nagłówek

                doc.Add(new Paragraph("Program Studiów".ToUpper())
                                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                doc.SetFontSize(12);
                #endregion

                #region dane podstawowe

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
                #endregion

                #region tableka opis programu studiów

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
                #endregion

                #region opis szczegółowy



                int w = lods[LearningOutcomeCategory.Knowledge];
                int u = lods[LearningOutcomeCategory.Skills];
                int k = lods[LearningOutcomeCategory.SocialCompetences];

                doc.Add(new Paragraph("2.   Opis szczegółowy"));

                doc.Add(new Paragraph($"2.1   Całkowita liczba efektów uczenia się w programie studiów: W(wiedza)= {w}, U (umiejętności) = {u}, K (kompetencje) = {k}, W + U + K = {w + u + k}"));

                doc.Add(new Paragraph($"2.2   Dla kierunku studiów przyporządkowanego do więcej niż jednej dyscypliny - liczba efektów uczenia się przypisana do dyscypliny \n\t D1(wiodąca){syllabus.FieldOfStudy.Discipline}: {w + u + k}"));

                doc.Add(new Paragraph($"2.3   Dla kierunku studiów przyporządkowanego do więcej niż jednej dyscypliny - procentowy udział liczby punktów ECTS dla każdej z dyscyplin: \n\t D1 100% punktów ECTS"));

                int suma = syllabus.SubjectDescriptions.Select(s => new { kind = s.Subject.ModuleType, ects = s.Subject.Lessons.Sum(l => l.Ects) })
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

                int obligatoryBasicScienceEcts = syllabus.SubjectDescriptions.Where(sd => sd.Subject.ModuleType == ModuleType.BasicScience
                                                                                       && sd.Subject.TypeOfSubject == TypeOfSubject.Obligatory)
                                                                             .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));

                int electiveBasicScienceEcts = syllabus.SubjectDescriptions.Where(sd => sd.Subject.ModuleType == ModuleType.BasicScience
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

                #endregion

                #region lista modułów - tabele z przedmiotami

                doc.Add(new Paragraph("Lista modułów kształcenia").SetFontSize(14));

                // Moduły obowiązkowe
                doc.Add(new Paragraph("4.1.   Lista modułów obowiązkowych").SetFontSize(14));

                // kształcenie ogólne
                doc.Add(new Paragraph("4.1.1.   Lista modułów kształcenia ogólnego").SetFontSize(13));
                // humanistyczne
                doc.Add(new Paragraph("4.1.1.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                    "przedmioty z obszaru nauk humanistycznych").SetItalic().SetFontSize(13)));
                doc.Add(moduleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Humanistic).Select(s => s.Subject).ToList()));

                doc.Add(Info);

                // naki podstawowe
                doc.Add(new Paragraph("4.1.2.   Lista modułów z zaktesu nauk podstawowych").SetFontSize(13));
                // matematyka
                doc.Add(new Paragraph("4.1.2.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                    "Matematyka").SetItalic().SetFontSize(13)));
                doc.Add(moduleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Maths).Select(s => s.Subject).ToList()));
                // fizyka
                doc.Add(new Paragraph("4.1.2.2.   Moduł ").SetFontSize(13).Add(new Paragraph(
                    "Fizyka").SetItalic().SetFontSize(13)));
                doc.Add(moduleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Physics).Select(s => s.Subject).ToList()));

                doc.Add(Info);

                // Moduły wybieralne
                doc.Add(new Paragraph("4.2.   Lista modułów wybieralnych").SetFontSize(14));

                // kształcenie ogólne
                doc.Add(new Paragraph("4.2.1.   Lista modułów kształcenia ogólnego").SetFontSize(13));
                // humanistyczne
                doc.Add(new Paragraph("4.2.1.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                    "Języki obce").SetItalic().SetFontSize(13)));
                doc.Add(moduleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.ForeignLanguage).Select(s => s.Subject).ToList()));

                doc.Add(Info);

                // kształcenie ogólne
                doc.Add(new Paragraph("4.2.2.   Lista modułów kształcenia ogólnego").SetFontSize(13));
                // kierunkowe wybieralne
                doc.Add(new Paragraph("4.2.2.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                    "kierunkowe wybieralne").SetItalic().SetFontSize(13)));
                doc.Add(moduleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.ModuleType == ModuleType.Thesis).Select(s => s.Subject).ToList()));

                doc.Add(Info);

                // specjalnosciowe
                doc.Add(new Paragraph("4.2.3.   Lista modułów specjalnościowych").SetFontSize(13));
                // kierunkowe wybieralne
                doc.Add(new Paragraph("4.2.3.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                    "Przedmioty specjalnościowe").SetItalic().SetFontSize(13)));
                doc.Add(moduleTable(syllabus.SubjectDescriptions.Where(s => s.Subject.ModuleType == ModuleType.Specialization
                                                                         || s.Subject.ModuleType == ModuleType.FieldOfStudy).Select(s => s.Subject).ToList()));

                doc.Add(Info);

                #endregion

                #region blok praktyk i pracy dyplomowej
                // blok praktyk
                doc.Add(new Paragraph("4.3.   Blok praktyk ").SetFontSize(13));
                doc.Add(new Paragraph(syllabus.IntershipType));
                // blok praca dyplomowa
                doc.Add(new Paragraph("4.4.   \"Praca dyplomowa\"").SetFontSize(13));
                doc.Add(new Paragraph(syllabus.ThesisCourse));
                #endregion

                #region sposoby weryfikacji

                doc.Add(new Paragraph("5.   Sposoby weryfikacji zakładanych efektów uczenia się ").SetFontSize(13));
                Table formOfCreditTable = new Table(2).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                formOfCreditTable.AddCell(ParagraphCell("Typ zajęć"));
                formOfCreditTable.AddCell(ParagraphCell("Sposoby weryfikacji zakładanych efektów uczenia się").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("wykład"));
                formOfCreditTable.AddCell(ParagraphCell("np. egzamin, kolokwium").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("ćwiczenia"));
                formOfCreditTable.AddCell(ParagraphCell("np. test, kolokwium").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("laboratorium"));
                formOfCreditTable.AddCell(ParagraphCell("np. wejściówka, sprawozdanie z laboratorium").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("projekt"));
                formOfCreditTable.AddCell(ParagraphCell("np. obrona projektu").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("seminarium"));
                formOfCreditTable.AddCell(ParagraphCell("np. udział w dyskusji, prezentacja tematu, esej").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("praktyka"));
                formOfCreditTable.AddCell(ParagraphCell("np. raport z praktyki").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                formOfCreditTable.AddCell(ParagraphCell("praca dyplomowa"));
                formOfCreditTable.AddCell(ParagraphCell("przygotowana praca dyplomowa").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                doc.Add(formOfCreditTable);
                #endregion

                #region zakres egzaminu

                doc.Add(new Paragraph("6.   Zakres egzaminu dyplomowego").SetFontSize(13));
                doc.Add(new Paragraph(syllabus.ScopeOfDiplomaExam));

                #endregion

                #region tabela z semestrami do kiedy zaliczyc
                doc.Add(new Paragraph("7.   Wymagania dotyczące terminu zaliczenia określonych kursów/grup kursów lub wszystkich kursów w poszczególnych modułach").SetFontSize(13));

                Table completitionTable = new Table(4).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                completitionTable.AddCell(new Paragraph("Lp."));
                completitionTable.AddCell(new Paragraph("Kod kursu"));
                completitionTable.AddCell(new Paragraph("Nazwa kursu"));
                completitionTable.AddCell(new Paragraph("Termin zaliczenia do... (numer semestru)"));
                int i = 1;
                syllabus.SubjectDescriptions.ForEach(sd =>
                {
                    completitionTable = completitionCell(completitionTable, sd, i);
                });

                doc.Add(completitionTable);
                #endregion
            }
            PdfHelper.AddPages();
        }
        public void Create(Syllabus syllabus)
        {
            using (Document doc = PdfHelper.Document(true))
            {

                #region setup i nagłówek
                const string lorem = "Lorem ipsum dolor sit amet lorem. Praesent lacinia at, egestas at, volutpat ut, condimentum dignissim. Pellentesque nunc. Praesent gravida justo, posuere urna vitae sem. Pellentesque fringilla ligula eleifend ac, eleifend erat volutpat. Pellentesque aliquam enim. Donec ullamcorper, risus metus eleifend neque ultrices iaculis. In vitae arcu erat, molestie nulla bibendum risus. Suspendisse vel hendrerit tellus et leo. Vivamus orci sit amet, euismod eget, rutrum ligula, et netus et netus et magnis dis parturient montes, nascetur ridiculus mus. Integer mi risus, pellentesque eget, bibendum ac, molestie tincidunt. Pellentesque euismod nulla fermentum vel, arcu. Etiam vel turpis vitae lacus. Ut sed enim. ";

                Paragraph info = new Paragraph("BK – liczba punktów ECTS przypisanych godzinom zajęć wymagających bezpośredniego kontaktu nauczycieli i studentów" +
                                               "\nTradycyjna – T, zdalna – Z" +
                                               "\nEgzamin -E, zaliczenie na ocenę – Z. W grupie kursów po literze E lub Z w nawiasie wpisać formę kursu końcowego (w,c,l,s,p) " +
                                               "\nKurs/grupa kursów Ogólnouczelniany – O" +
                                               "\nKurs/grupa kursów Praktyczny – P. W grupie kursów w nawiasie wpisać liczbę punktów ECTS dla kursów o charakterze praktycznym, " +
                                               "\nKO – kształcenia ogólnego, PH – podstawowy, K – kierunkowy, S – specjalnościowy" +
                                               "\nW – wybieralny, OB. – obowiązkowy").SetFontSize(7);


                doc.Add(new Paragraph("Plan studiów".ToUpper())
                                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                doc.SetFontSize(12);
                #endregion

                #region dane podstawowe

                // Wydział
                doc.Add(new Paragraph("Wydział ".ToUpper() + syllabus.FieldOfStudy.Faculty));

                // kierunek
                doc.Add(new Paragraph("Kierunek studiów: ".ToUpper())
                        .Add(new Paragraph(syllabus.FieldOfStudy.Name)
                            .SetItalic()
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));


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

                // Profil
                doc.Add(new Paragraph("Specjalność: ".ToUpper())
                        .Add(new Paragraph(syllabus.Specialization.Name)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));

                // Język
                doc.Add(new Paragraph("Język prowadzenia studiów: ".ToUpper())
                        .Add(new Paragraph(EnumTranslator.Translate(syllabus.FieldOfStudy.Language.ToString()))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                #endregion

                #region tabele z przedmiotami na semestr

                doc.Add(new Paragraph("1. Zestaw kursów i grup kursów obowiązkowych i wybieralnych w układzie semestralnym").SetBold());

                for (int i = 1; i <= syllabus.Description.NumOfSemesters; i++)
                {

                    doc.Add(new Paragraph($"Semestr {i}").SetBold());

                    List<Subject> semesterSubjects = syllabus.SubjectDescriptions.Where(s => s.AssignedSemester == i)
                                                                       .Select(s => s.Subject)
                                                                       .ToList();

                    List<Subject> obligatorySubjects = semesterSubjects.Where(s => s.TypeOfSubject == TypeOfSubject.Obligatory)
                                                             .ToList();

                    List<Subject> electiveSubjects = semesterSubjects.Where(s => s.TypeOfSubject == TypeOfSubject.Elective)
                                                           .ToList();

                    doc.Add(new Paragraph($"Kursy obowiązkowe: {obligatorySubjects.Sum(s => s.Lessons.Sum(l => l.HoursAtUniversity))}      liczba punktów ECTS {obligatorySubjects.Sum(s => s.Lessons.Sum(l => l.Ects))}").SetBold());
                    doc.Add(moduleTable(obligatorySubjects));

                    doc.Add(new Paragraph($"Kursy wybieralne: {electiveSubjects.Sum(s => s.Lessons.Sum(l => l.HoursAtUniversity))}      liczba punktów ECTS {electiveSubjects.Sum(s => s.Lessons.Sum(l => l.Ects))}").SetBold());
                    doc.Add(moduleTable(electiveSubjects));

                    doc.Add(new Paragraph($"Razem w semestrze: "));

                    doc.Add(semesterSummaryTable(semesterSubjects));

                    doc.Add(info);
                }

                #endregion

                #region deficyt
                doc.Add(new Paragraph("2. Liczby dopuszczalnego deficytu punktów ECTS po poszczególnych semestrach").SetBold());
                Table deficitTable = new Table(2).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                                                          .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                deficitTable.AddCell("Semestr");
                deficitTable.AddCell("Dopuszczalny deficyt punktów ECTS po semestrze");

                for (int i = 1; i < syllabus.Description.NumOfSemesters; i++)
                {
                    deficitTable.AddCell(i.ToString());
                    deficitTable.AddCell("8");
                }

                doc.Add(deficitTable);
                #endregion

                #region Opinia
                doc.Add(new Paragraph());
                doc.Add(new Paragraph("Opinia wydziałowego organu uchwałodawczego samorządu studenckiego"));
                Table opinionTable = new Table(3);
                opinionTable.SetBorder(Border.NO_BORDER);

                opinionTable.AddCell(ParagraphCell("...............").SetBorder(Border.NO_BORDER));
                opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f));
                opinionTable.AddCell(ParagraphCell(".............................................").SetBorder(Border.NO_BORDER));

                opinionTable.AddCell(ParagraphCell("Data").SetBorder(Border.NO_BORDER));
                opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f));
                opinionTable.AddCell(ParagraphCell("Imię, nazwisko i podpis przedstawiciela studentów").SetBorder(Border.NO_BORDER));

                opinionTable.AddCell(ParagraphCell("...............").SetBorder(Border.NO_BORDER));
                opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f)).SetBorder(Border.NO_BORDER);
                opinionTable.AddCell(ParagraphCell(".............................................").SetBorder(Border.NO_BORDER));

                opinionTable.AddCell(ParagraphCell("Data").SetBorder(Border.NO_BORDER));
                opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f));
                opinionTable.AddCell(ParagraphCell("Podpis Dziekana").SetBorder(Border.NO_BORDER));

                doc.Add(opinionTable);
                #endregion

            }

            PdfHelper.AddPages();
        }



        #region Learning outcome
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
                    foreach (string item in c)
                    {
                        tab.AddCell(new Cell().Add(new Paragraph(item)));
                    }
                });
            }
        }

        #endregion
        public void Create(LearningOutcomeDocument lod)
        {
            FieldOfStudy fos = lod.FieldOfStudy;
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
                            string propName = ((PdfNameAttribute)prop.GetCustomAttribute(typeof(PdfNameAttribute), true)).name;
                            string value = prop.GetValue(fos)?.ToString() ?? "";

                            doc.Add(new Paragraph(EnumTranslator.Translate(propName) + " - " + EnumTranslator.Translate(value)));
                        }
                    }
                }

                if (lods != null)
                {
                    List<string> headers = new List<string>();
                    List<List<string>> cellsKnowledge = new List<List<string>>();
                    List<List<string>> cellsSkills = new List<List<string>>();
                    List<List<string>> cellsSocialCompetences = new List<List<string>>();

                    List<CategoryTableModel> categoryTableModels = new List<CategoryTableModel>();


                    foreach (PropertyInfo prop in typeof(LearningOutcome).GetProperties())
                    {
                        if (Attribute.IsDefined(prop, typeof(PdfNameAttribute)))
                        {
                            string propName = ((PdfNameAttribute)prop.GetCustomAttribute(typeof(PdfNameAttribute), false)).name;
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
                                    cellsKnowledge.Add(cell);
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

                            if (categoryTableModels.Any(x => x.Name == l.Specialization.Name) == false)
                            {
                                categoryTableModels.Add(new CategoryTableModel(l.Specialization.Name));
                            }
                            CategoryTableModel categoryTableModel = categoryTableModels.FirstOrDefault(x => x.Name == l.Specialization.Name);

                            switch (l.Category)
                            {
                                case LearningOutcomeCategory.Knowledge:
                                    categoryTableModel.SpecKnowledge.Add(cell);
                                    break;
                                case LearningOutcomeCategory.Skills:
                                    categoryTableModel.SpecSkills.Add(cell);
                                    break;
                                case LearningOutcomeCategory.SocialCompetences:
                                    categoryTableModel.SpecSocialCompetences.Add(cell);
                                    break;
                                default:
                                    break;
                            }

                        }


                    });

                    Table loTable = new Table(headers.Count);
                    headers.ForEach(h => loTable.AddHeaderCell(h));


                    setCells(LearningOutcomeCategory.Knowledge, cellsKnowledge, headers.Count, loTable);
                    setCells(LearningOutcomeCategory.Skills, cellsSkills, headers.Count, loTable);
                    setCells(LearningOutcomeCategory.SocialCompetences, cellsSocialCompetences, headers.Count, loTable);

                    doc.Add(loTable.SetFontSize(9));

                    Table specTable = new Table(headers.Count);

                    categoryTableModels.ForEach(ctm =>
                    {
                        doc.Add(new Paragraph("Specjalność - " + ctm.Name));
                        setCells(LearningOutcomeCategory.Knowledge, ctm.SpecKnowledge, headers.Count, specTable);
                        setCells(LearningOutcomeCategory.Skills, ctm.SpecSkills, headers.Count, specTable);
                        setCells(LearningOutcomeCategory.SocialCompetences, ctm.SpecSocialCompetences, headers.Count, specTable);
                    });

                    doc.Add(specTable.SetFontSize(9));

                }
                PdfHelper.AddPages();
            }
        }
        
        
        #region Subject
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
        public void Create(Subject subject)
        {
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
                doc.Add(new Paragraph("Kod przedmiotu: ")
                        .Add(new Paragraph(subject.Code.ToUpper())
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                        .SetFixedLeading(0.5f));
                // grupa
                bool isGroup = subject.Lessons.Any(l => l.IsGroup);

                doc.Add(new Paragraph("Grupa kursów: ")
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

                Lesson Lecture = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Lecture);
                Lesson Classes = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Classes);
                Lesson Laboratory = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Laboratory);
                Lesson Project = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Project);
                Lesson Seminar = subject.Lessons.FirstOrDefault(l => l.LessonType == LessonType.Seminar);

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
                List<string> primary = subject.Literature.Where(l => l.IsPrimary).Select(l => l.Authors + ". " + l.Title + ", " + l.Distributor + " " + l.Year + ". " + l.Isbn).ToList();
                List<string> secondary = subject.Literature.Where(l => !l.IsPrimary).Select(l => l.Authors + ". " + l.Title + ", " + l.Distributor + " " + l.Year + ". " + l.Isbn).ToList();

                doc.Add(new Paragraph("LITERATURA PODSTAWOWA:")
                    .SetUnderline());
                doc.Add(PdfHelper.List(primary));

                doc.Add(new Paragraph("LITERATURA UZUPEŁNIAJĄCA:")
                    .SetUnderline());
                doc.Add(PdfHelper.List(secondary));
            }
            PdfHelper.AddPages();
        }


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
