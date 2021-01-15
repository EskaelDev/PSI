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
    public class SyllabusPdf : ISyllabusPdf
    {
        public void Create(Syllabus syllabus, Dictionary<LearningOutcomeCategory, int> lods)
        {
            using (Document doc = PdfHelper.Document(true))
            {
                var fieldOfStudy = syllabus.FieldOfStudy;
                var description = syllabus.Description;
                var subjectDescriptions = syllabus.SubjectDescriptions;
                #region setup i nagłówek

                syllabusTitle(doc);

                doc.SetFontSize(12);
                #endregion

                #region dane podstawowe
                PdfStructures.fieldOfStudyInfo(doc, fieldOfStudy);
                #endregion

                #region tableka opis programu studiów
                generalDescription(doc, description);
                #endregion

                #region opis szczegółowy
                wku(lods, doc, fieldOfStudy);
                disciplineEcts50(syllabus.SubjectDescriptions, doc);
                accordanceToJobMarket(doc);
                directContact(syllabus.SubjectDescriptions, doc);
                basicScienceTotallEcts(syllabus.SubjectDescriptions, doc);
                generalModulesMinimalEcts(syllabus.PointLimits, doc);
                electiveSubjectsTotalEcts(doc, subjectDescriptions);
                learningOutcomeProcess(doc);
                #endregion

                #region lista modułów - tabele z przedmiotami
                modulesTable(doc, subjectDescriptions);
                #endregion

                #region blok praktyk i pracy dyplomowej
                // blok praktyk
                internAndThesis(syllabus.IntershipType, syllabus.ThesisCourse, doc);
                #endregion

                #region sposoby weryfikacji
                formOfCreditTable(doc);
                #endregion

                #region zakres egzaminu
                examScope(syllabus.ScopeOfDiplomaExam, doc);
                #endregion

                #region tabela z semestrami do kiedy zaliczyc
                semesterCompletitionTable(doc, subjectDescriptions);
                #endregion
            }
            PdfHelper.AddPages();


        }
        #region Syllabus

       

        private void semesterCompletitionTable(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            if (subjectDescriptions.Count > 0)
            {
                doc.Add(new Paragraph("7.   Wymagania dotyczące terminu zaliczenia określonych kursów/grup kursów lub wszystkich kursów w poszczególnych modułach").SetFontSize(13));

                Table completitionTable = new Table(4).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                completitionTable.AddCell(new Paragraph("Lp."));
                completitionTable.AddCell(new Paragraph("Kod kursu"));
                completitionTable.AddCell(new Paragraph("Nazwa kursu"));
                completitionTable.AddCell(new Paragraph("Termin zaliczenia do... (numer semestru)"));
                int i = 1;
                subjectDescriptions.ForEach(sd =>
                {
                    completitionTable = PdfStructures.completitionCell(completitionTable, sd, i);
                });

                doc.Add(completitionTable);
            }
        }

        private static void examScope(string ScopeOfDiplomaExam, Document doc)
        {
            doc.Add(new Paragraph("6.   Zakres egzaminu dyplomowego").SetFontSize(13));
            doc.Add(new Paragraph(ScopeOfDiplomaExam));
        }

        private void formOfCreditTable(Document doc)
        {
            doc.Add(new Paragraph("5.   Sposoby weryfikacji zakładanych efektów uczenia się ").SetFontSize(13));
            Table formOfCreditTable = new Table(2).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("Typ zajęć"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("Sposoby weryfikacji zakładanych efektów uczenia się").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("wykład"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("np. egzamin, kolokwium").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("ćwiczenia"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("np. test, kolokwium").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("laboratorium"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("np. wejściówka, sprawozdanie z laboratorium").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("projekt"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("np. obrona projektu").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("seminarium"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("np. udział w dyskusji, prezentacja tematu, esej").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("praktyka"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("np. raport z praktyki").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("praca dyplomowa"));
            formOfCreditTable.AddCell(PdfStructures.ParagraphCell("przygotowana praca dyplomowa").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

            doc.Add(formOfCreditTable);
        }

        private static void internAndThesis(string IntershipType, string ThesisCourse, Document doc)
        {
            if (IntershipType != null)
            {
                doc.Add(new Paragraph("4.3.   Blok praktyk ").SetFontSize(13));
                doc.Add(new Paragraph(IntershipType));
            }
            // blok praca dyplomowa
            if (ThesisCourse != null)
            {
                doc.Add(new Paragraph("4.4.   Praca dyplomowa").SetFontSize(13));
                doc.Add(new Paragraph(ThesisCourse));
            }
        }

        private void modulesTable(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("Lista modułów kształcenia").SetFontSize(14));

            // Moduły obowiązkowe
            obligatoryModules(doc, subjectDescriptions);

            // Moduły wybieralne
            electiveModules(doc, subjectDescriptions);

            // kształcenie ogólne
            thesisModules(doc, subjectDescriptions);

            // specjalnosciowe
            specializationModules(doc, subjectDescriptions);
        }

        private void electiveModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.2.   Lista modułów wybieralnych").SetFontSize(14));

            // kształcenie ogólne
            doc.Add(new Paragraph("4.2.1.   Lista modułów kształcenia ogólnego").SetFontSize(13));
            // humanistyczne
            foreignLanguageModules(doc, subjectDescriptions);

            doc.Add(PdfStructures.Info);
        }

        private void obligatoryModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.1.   Lista modułów obowiązkowych").SetFontSize(14));

            // kształcenie ogólne
            generalModules(doc, subjectDescriptions);
        }

        private void thesisModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.2.2.   Lista modułów kształcenia ogólnego").SetFontSize(13));
            // kierunkowe wybieralne
            doc.Add(new Paragraph("4.2.2.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                "kierunkowe wybieralne").SetItalic().SetFontSize(13)));
            doc.Add(PdfStructures.moduleTable(subjectDescriptions.Where(s => s.Subject.ModuleType == ModuleType.Thesis).Select(s => s.Subject).ToList()));

            doc.Add(PdfStructures.Info);
        }

        private void specializationModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.2.3.   Lista modułów specjalnościowych").SetFontSize(13));
            // kierunkowe wybieralne
            doc.Add(new Paragraph("4.2.3.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                "Przedmioty specjalnościowe").SetItalic().SetFontSize(13)));
            doc.Add(PdfStructures.moduleTable(subjectDescriptions.Where(s => s.Subject.ModuleType == ModuleType.Specialization
                                                                     || s.Subject.ModuleType == ModuleType.FieldOfStudy).Select(s => s.Subject).ToList()));

            doc.Add(PdfStructures.Info);
        }

        private void foreignLanguageModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.2.1.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                                "Języki obce").SetItalic().SetFontSize(13)));
            doc.Add(PdfStructures.moduleTable(subjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.ForeignLanguage).Select(s => s.Subject).ToList()));
        }

        private void generalModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.1.1.   Lista modułów kształcenia ogólnego").SetFontSize(13));
            // humanistyczne
            humanisticSubjects(doc, subjectDescriptions);

            // naki podstawowe
            mathAndPhysicModules(doc, subjectDescriptions);
        }

        private void mathAndPhysicModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.1.2.   Lista modułów z zaktesu nauk podstawowych").SetFontSize(13));
            // matematyka
            mathModules(doc, subjectDescriptions);
            // fizyka
            physicModules(doc, subjectDescriptions);

            doc.Add(PdfStructures.Info);
        }

        private void physicModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.1.2.2.   Moduł ").SetFontSize(13).Add(new Paragraph(
                                "Fizyka").SetItalic().SetFontSize(13)));
            doc.Add(PdfStructures.moduleTable(subjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Physics).Select(s => s.Subject).ToList()));
        }

        private void mathModules(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.1.2.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                                "Matematyka").SetItalic().SetFontSize(13)));
            doc.Add(PdfStructures.moduleTable(subjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Maths).Select(s => s.Subject).ToList()));
        }

        private void humanisticSubjects(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            doc.Add(new Paragraph("4.1.1.1.   Moduł ").SetFontSize(13).Add(new Paragraph(
                                "przedmioty z obszaru nauk humanistycznych").SetItalic().SetFontSize(13)));
            doc.Add(PdfStructures.moduleTable(subjectDescriptions.Where(s => s.Subject.KindOfSubject == KindOfSubject.Humanistic).Select(s => s.Subject).ToList()));

            doc.Add(PdfStructures.Info);
        }

        private static void learningOutcomeProcess(Document doc)
        {
            doc.Add(new Paragraph("3.   Opis procesu prowadzącego do uzyskania efektów uczenia się:"));
            doc.Add(new Paragraph(PdfStructures.Lorem));
        }

        private static void electiveSubjectsTotalEcts(Document doc, List<SubjectInSyllabusDescription> subjectDescriptions)
        {
            int totalElectiveEcts = subjectDescriptions.Where(sd => sd.Subject.TypeOfSubject == TypeOfSubject.Elective)
                                                                                .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));
            doc.Add(new Paragraph("2.10   Łączna liczba punktów ECTS, którą student może uzyskać, realizując bloki wybieralne (min. 30% całkowitej liczby punktów ECTS) ")
                                .Add(new Paragraph($"{totalElectiveEcts} ECTS")));
        }

        private static void generalModulesMinimalEcts(List<PointLimit> PointLimits, Document doc)
        {
            int minGeneralEcts = PointLimits.FirstOrDefault(pl => pl.ModuleType == ModuleType.General).Points;
            doc.Add(new Paragraph("2.9   Minimalna liczba punktów ECTS, którą student musi uzyskać, realizując bloki kształcenia oferowane na zajęciach ogólnouczelnianych lub na innym kierunku studiów (wpisać sumę ECTS kursów/grup kursów oznaczonych kodem O) ")
                                .Add(new Paragraph($"{minGeneralEcts} ECTS")));
        }

        private static void syllabusTitle(Document doc)
        {
            doc.Add(new Paragraph("Program Studiów".ToUpper())
                                                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
        }

        private static void basicScienceTotallEcts(List<SubjectInSyllabusDescription> SubjectDescriptions, Document doc)
        {
            doc.Add(new Paragraph("2.7   Łączna liczba punktów ECTS, którą student musi uzyskać w ramach zajęć z zakresu nauk podstawowych"));

            int obligatoryBasicScienceEcts = SubjectDescriptions.Where(sd => sd.Subject.ModuleType == ModuleType.BasicScience
                                                                                   && sd.Subject.TypeOfSubject == TypeOfSubject.Obligatory)
                                                                         .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));

            int electiveBasicScienceEcts = SubjectDescriptions.Where(sd => sd.Subject.ModuleType == ModuleType.BasicScience
                                                                                 && sd.Subject.TypeOfSubject == TypeOfSubject.Elective)
                                                                       .Sum(sd => sd.Subject.Lessons.Sum(l => l.Ects));
            doc.Add(new Paragraph($"Liczba punktów ECTS z przedmiotów wybieralnych:   {obligatoryBasicScienceEcts}"));
            doc.Add(new Paragraph($"Liczba punktów ECTS z przedmiotów obowiązkowych:   {electiveBasicScienceEcts}"));
            doc.Add(new Paragraph($"Łączna liczba punktów ECTS:   {obligatoryBasicScienceEcts + electiveBasicScienceEcts}"));
        }

        private static void directContact(List<SubjectInSyllabusDescription> SubjectDescriptions, Document doc)
        {
            int directEcts = SubjectDescriptions.Sum(sd => sd.Subject.Lessons.Sum(l => l.EctsinclDirectTeacherStudentContactClasses));
            doc.Add(new Paragraph("2.6   Łączna liczba punktów ECTS, którą student musi uzyskać na zajęciach wymagających bezpośredniego udziału nauczycieli akademickich lub innych osób prowadzących zajęcia i studentów (wpisać sumę punktów ECTS dla kursów/grup kursów oznaczonych kodem BK)")
                .Add(new Paragraph(directEcts.ToString() + " Punkty ECTS")));
        }

        private static void accordanceToJobMarket(Document doc)
        {
            doc.Add(new Paragraph("2.5   Zwięzła analiza zgodności zakładanych efektów uczenia się z potrzebami rynku pracy"));

            doc.Add(new Paragraph(PdfStructures.Lorem));
        }

        private static void disciplineEcts50(List<SubjectInSyllabusDescription> SubjectDescriptions, Document doc)
        {
            int suma = SubjectDescriptions.Select(s => new { kind = s.Subject.ModuleType, ects = s.Subject.Lessons.Sum(l => l.Ects) })
                                                                   .Where(s => s.kind == ModuleType.General) // ToDO: Możliwe że trzeba dodać typy
                                                                   .Sum(s => s.ects);
            doc.Add(new Paragraph($"2.4a   Dla kierunku studiów o profilu ogólnoakademickim - liczba punktów ECTS przypisana zajęciom związanym z prowadzoną na Uczelni działalnością naukową w dyscyplinie lub dyscyplinach, do których przeyporządkowany jest kierunek studiów")
                                        .Add(new Paragraph("musi być większa niż 50% całkowitej liczby punktów ECTS z p 1.1  ").SetFontSize(10))
                                        .Add(new Paragraph(suma.ToString())));
        }

        private static void wku(Dictionary<LearningOutcomeCategory, int> lods, Document doc, FieldOfStudy fieldOfStudy)
        {
            int w = lods[LearningOutcomeCategory.Knowledge];
            int u = lods[LearningOutcomeCategory.Skills];
            int k = lods[LearningOutcomeCategory.SocialCompetences];

            doc.Add(new Paragraph("2.   Opis szczegółowy"));

            doc.Add(new Paragraph($"2.1   Całkowita liczba efektów uczenia się w programie studiów: W(wiedza)= {w}, U (umiejętności) = {u}, K (kompetencje) = {k}, W + U + K = {w + u + k}"));

            doc.Add(new Paragraph($"2.2   Dla kierunku studiów przyporządkowanego do więcej niż jednej dyscypliny - liczba efektów uczenia się przypisana do dyscypliny \n\t D1(wiodąca){fieldOfStudy.Discipline}: {w + u + k}"));

            doc.Add(new Paragraph($"2.3   Dla kierunku studiów przyporządkowanego do więcej niż jednej dyscypliny - procentowy udział liczby punktów ECTS dla każdej z dyscyplin: \n\t D1 100% punktów ECTS"));
        }

        private void generalDescription(Document doc, SyllabusDescription description)
        {
            doc.Add(new Paragraph("1. Opis ogólny").SetBold());
            Table generalInfoTable = new Table(2);

            numberOfSemesters(description, generalInfoTable);
            ectsToFinishStudy(description, generalInfoTable);

            hoursTotal(generalInfoTable);
            prerequisites(description, generalInfoTable);
            titleAfterGraduation(description, generalInfoTable);
            employmentOpportinities(description, generalInfoTable);
            possibilityOfContinuation(description, generalInfoTable);
            universityMission(generalInfoTable);

            doc.Add(generalInfoTable);
        }

        private void universityMission(Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                                PdfStructures.generalInfoCell(
                                    "1.8 Wskazanie związku z misją uczelni i strategią jej rozwoju:",
                                    PdfStructures.Lorem)); // ToDo: coś z tym zrobić, usunąć albo nie wiem
        }

        private void possibilityOfContinuation(SyllabusDescription description, Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                                PdfStructures.generalInfoCell(
                                    "1.7 Możliwość kontynuacji studiów:",
                                    description.PossibilityOfContinuation));
        }

        private void employmentOpportinities(SyllabusDescription description, Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                                PdfStructures.generalInfoCell(
                                    "1.6 Sylwetka absolwenta, możliwości zatrudnienia:",
                                    description.EmploymentOpportunities));
        }

        private void titleAfterGraduation(SyllabusDescription description, Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                                PdfStructures.generalInfoCell(
                                    "1.5 Tytuł zawodowy nadawany po zakończeniu studiów:",
                                    EnumTranslator.Translate(description.ProfessionalTitleAfterGraduation.ToString()),
                                    true));
        }

        private void prerequisites(SyllabusDescription description, Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                                PdfStructures.generalInfoCell(
                                    "1.4 Wymagania wstępne:",
                                    description.Prerequisites));
        }

        private void hoursTotal(Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                PdfStructures.generalInfoCell(
                    "1.3 Łączna liczba godzin zajęć:",
                    "1020", // TODO: znaleźć pole od tego
                    true));
        }

        private void ectsToFinishStudy(SyllabusDescription description, Table generalInfoTable)
        {
            generalInfoTable.AddCell(
                                PdfStructures.generalInfoCell(
                                    "1.2 Całkowita liczba punktów ECTS konieczna do ukończenia studiów na danym poziomie:",
                                    description.Ects.ToString(),
                                    true));
        }

        private void numberOfSemesters(SyllabusDescription description, Table generalInfoTable)
        {
            generalInfoTable.AddCell(PdfStructures.generalInfoCell("1.1 Liczba semestrów:",
                description.NumOfSemesters.ToString(),
                true));
        }

    #endregion

    }
}
