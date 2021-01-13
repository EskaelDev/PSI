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
    public class PlanPdf : IPlanPdf
    {
        public void Create(Syllabus syllabus)
        {
            using (Document doc = PdfHelper.Document(true))
            {
                var fieldOfStudy = syllabus.FieldOfStudy;
                var description = syllabus.Description;
                var subjectDescriptions = syllabus.SubjectDescriptions;

                #region setup i nagłówek
                planTitle(doc);
                doc.SetFontSize(12);
                #endregion


                #region dane podstawowe
                if (fieldOfStudy != null)
                    PdfStructures.fieldOfStudyInfo(doc, fieldOfStudy);
                // Specjalność
                if (syllabus.Specialization != null)
                    specialization(syllabus.Specialization.Name, doc);
                #endregion

                #region tabele z przedmiotami na semestr
                if (subjectDescriptions != null)
                    subjectTables(subjectDescriptions, description.NumOfSemesters, doc);
                #endregion

                #region deficyt
                if (description != null)
                deficit(doc, description);
                #endregion

                #region Opinia
                opinion(doc);
                #endregion

            }

            PdfHelper.AddPages();
        }

        private static void planTitle(Document doc)
        {
            doc.Add(new Paragraph("Plan studiów".ToUpper())
                                                 .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
        }

        private static void opinion(Document doc)
        {
            doc.Add(new Paragraph());
            doc.Add(new Paragraph("Opinia wydziałowego organu uchwałodawczego samorządu studenckiego"));
            Table opinionTable = new Table(3);
            opinionTable.SetBorder(Border.NO_BORDER);

            opinionTable.AddCell(PdfStructures.ParagraphCell("...............").SetBorder(Border.NO_BORDER));
            opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f));
            opinionTable.AddCell(PdfStructures.ParagraphCell(".............................................").SetBorder(Border.NO_BORDER));

            opinionTable.AddCell(PdfStructures.ParagraphCell("Data").SetBorder(Border.NO_BORDER));
            opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f));
            opinionTable.AddCell(PdfStructures.ParagraphCell("Imię, nazwisko i podpis przedstawiciela studentów").SetBorder(Border.NO_BORDER));

            opinionTable.AddCell(PdfStructures.ParagraphCell("...............").SetBorder(Border.NO_BORDER));
            opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f)).SetBorder(Border.NO_BORDER);
            opinionTable.AddCell(PdfStructures.ParagraphCell(".............................................").SetBorder(Border.NO_BORDER));

            opinionTable.AddCell(PdfStructures.ParagraphCell("Data").SetBorder(Border.NO_BORDER));
            opinionTable.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetWidth(300f));
            opinionTable.AddCell(PdfStructures.ParagraphCell("Podpis Dziekana").SetBorder(Border.NO_BORDER));

            doc.Add(opinionTable);
        }

        private static void deficit(Document doc, SyllabusDescription description)
        {
            doc.Add(new Paragraph("2. Liczby dopuszczalnego deficytu punktów ECTS po poszczególnych semestrach").SetBold());
            Table deficitTable = new Table(2).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                                                      .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            deficitTable.AddCell("Semestr");
            deficitTable.AddCell("Dopuszczalny deficyt punktów ECTS po semestrze");

            for (int i = 1; i < description.NumOfSemesters; i++)
            {
                deficitTable.AddCell(i.ToString());
                deficitTable.AddCell("8");
            }

            doc.Add(deficitTable);
        }

        private static void subjectTables(List<SubjectInSyllabusDescription> SubjectDescriptions, int NumOfSemesters, Document doc)
        {
            doc.Add(new Paragraph("1. Zestaw kursów i grup kursów obowiązkowych i wybieralnych w układzie semestralnym").SetBold());

            for (int i = 1; i <= NumOfSemesters; i++)
            {
                subjectsPerSemesterTable(SubjectDescriptions, doc, i);
            }
        }

        private static void subjectsPerSemesterTable(List<SubjectInSyllabusDescription> SubjectDescriptions, Document doc, int i)
        {
            doc.Add(new Paragraph($"Semestr {i}").SetBold());

            List<Subject> semesterSubjects = SubjectDescriptions.Where(s => s.AssignedSemester == i)
                                                                .Select(s => s.Subject)
                                                                .ToList();
            obligatorySubjects(doc, semesterSubjects);
            electiveSubjects(doc, semesterSubjects);

            doc.Add(new Paragraph($"Razem w semestrze: "));

            doc.Add(PdfStructures.semesterSummaryTable(semesterSubjects));

            doc.Add(PdfStructures.Info);
        }

        private static void electiveSubjects(Document doc, List<Subject> semesterSubjects)
        {
            List<Subject> electiveSubjects = semesterSubjects.Where(s => s.TypeOfSubject == TypeOfSubject.Elective)
                                                                                 .ToList();
            doc.Add(new Paragraph($"Kursy wybieralne: {electiveSubjects.Sum(s => s.Lessons.Sum(l => l.HoursAtUniversity))}      liczba punktów ECTS {electiveSubjects.Sum(s => s.Lessons.Sum(l => l.Ects))}").SetBold());
            doc.Add(PdfStructures.moduleTable(electiveSubjects));
        }

        private static void obligatorySubjects(Document doc, List<Subject> semesterSubjects)
        {
            List<Subject> obligatorySubjects = semesterSubjects.Where(s => s.TypeOfSubject == TypeOfSubject.Obligatory)
                                                                                   .ToList();
            doc.Add(new Paragraph($"Kursy obowiązkowe: {obligatorySubjects.Sum(s => s.Lessons.Sum(l => l.HoursAtUniversity))}      liczba punktów ECTS {obligatorySubjects.Sum(s => s.Lessons.Sum(l => l.Ects))}").SetBold());
            doc.Add(PdfStructures.moduleTable(obligatorySubjects));
        }

        private static void specialization(string Name, Document doc)
        {
            doc.Add(new Paragraph("Specjalność: ".ToUpper())
                    .Add(new Paragraph(Name)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT))
                    .SetFixedLeading(0.5f));
        }
    }
}
