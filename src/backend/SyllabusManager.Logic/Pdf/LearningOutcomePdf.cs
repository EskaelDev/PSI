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
    public class LearningOutcomePdf : ILearningOutcomePdf
    {

        public void Create(LearningOutcomeDocument lod)
        {
            FieldOfStudy fieldOfStudy = lod.FieldOfStudy;
            List<LearningOutcome> learningOutcomes = lod.LearningOutcomes;

            using (Document doc = PdfHelper.Document(true))
            {
                learningOutcomeTitle(lod, doc);

                if (fieldOfStudy != null)
                    fieldOfStudyInfo(fieldOfStudy, doc);


                if (learningOutcomes != null)
                    addLearningOutcomes(learningOutcomes, doc);
            }
            PdfHelper.AddPages();
        }

        private static void learningOutcomeTitle(LearningOutcomeDocument lod, Document doc)
        {
            doc.Add(new Paragraph("ZAKŁADANE EFEKTY UCZENIA SIĘ").SetFontSize(20));
            doc.Add(new Paragraph($"Rok akademicki: {lod.AcademicYear}"));
        }

        private void addLearningOutcomes(List<LearningOutcome> learningOutcomes, Document doc)
        {
            List<string> headers = new List<string>();
            List<List<string>> cellsKnowledge = new List<List<string>>();
            List<List<string>> cellsSkills = new List<List<string>>();
            List<List<string>> cellsSocialCompetences = new List<List<string>>();

            List<CategoryTableModel> categoryTableModels = new List<CategoryTableModel>();
            fillHeaders(headers);

            learningOutcomes.ForEach(l =>
            {
                List<string> cell = (typeof(LearningOutcome).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PdfNameAttribute)))
                                                                            .Select(prop => EnumTranslator.Translate(prop.GetValue(l)?.ToString() ?? "")))
                                                                            .ToList();
                fillSpecializationLists(l, cellsKnowledge, cellsSkills, cellsSocialCompetences, categoryTableModels, cell);

            });
            generalLearningOutcomeTable(doc, headers, cellsKnowledge, cellsSkills, cellsSocialCompetences);

            specializationLearningOutcomeTable(doc, headers, categoryTableModels);
        }

        private void specializationLearningOutcomeTable(Document doc, List<string> headers, List<CategoryTableModel> categoryTableModels)
        {
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

        private void generalLearningOutcomeTable(Document doc, List<string> headers, List<List<string>> cellsKnowledge, List<List<string>> cellsSkills, List<List<string>> cellsSocialCompetences)
        {
            Table loTable = new Table(headers.Count);
            headers.ForEach(h => loTable.AddHeaderCell(h));


            setCells(LearningOutcomeCategory.Knowledge, cellsKnowledge, headers.Count, loTable);
            setCells(LearningOutcomeCategory.Skills, cellsSkills, headers.Count, loTable);
            setCells(LearningOutcomeCategory.SocialCompetences, cellsSocialCompetences, headers.Count, loTable);

            doc.Add(loTable.SetFontSize(9));
        }

        private static void fillSpecializationLists(LearningOutcome l, List<List<string>> cellsKnowledge, List<List<string>> cellsSkills, List<List<string>> cellsSocialCompetences, List<CategoryTableModel> categoryTableModels, List<string> cell)
        {
            if (l.Specialization is null)
                fillCategoryLists(l, cellsKnowledge, cellsSkills, cellsSocialCompetences, cell);
            else
            {
                if (categoryTableModels.Any(x => x.Name == l.Specialization.Name) == false)
                {
                    categoryTableModels.Add(new CategoryTableModel(l.Specialization.Name));
                }
                CategoryTableModel categoryTableModel = categoryTableModels.FirstOrDefault(x => x.Name == l.Specialization.Name);
                fillCategoryLists(l, categoryTableModel.SpecKnowledge, categoryTableModel.SpecSkills, categoryTableModel.SpecSocialCompetences, cell);
            }
        }

        private static void fillCategoryLists(LearningOutcome l, List<List<string>> cellsKnowledge, List<List<string>> cellsSkills, List<List<string>> cellsSocialCompetences, List<string> cell)
        {
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
        }

        private static void fillHeaders(List<string> headers)
        {
            foreach (var prop in typeof(LearningOutcome).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PdfNameAttribute))))
            {
                string propName = ((PdfNameAttribute)prop.GetCustomAttribute(typeof(PdfNameAttribute), false)).name;
                headers.Add(propName);
            }
        }

        private static void fieldOfStudyInfo(FieldOfStudy fieldOfStudy, Document doc)
        {
            foreach (var prop in typeof(FieldOfStudy).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PdfNameAttribute))))
            {
                string propName = ((PdfNameAttribute)prop.GetCustomAttribute(typeof(PdfNameAttribute), true)).name;
                string value = prop.GetValue(fieldOfStudy)?.ToString() ?? "";
                doc.Add(new Paragraph(EnumTranslator.Translate(propName) + " - " + EnumTranslator.Translate(value)));
            }
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
                    foreach (string item in c)
                    {
                        tab.AddCell(new Cell().Add(new Paragraph(item)));
                    }
                });
            }
        }
    }
}
