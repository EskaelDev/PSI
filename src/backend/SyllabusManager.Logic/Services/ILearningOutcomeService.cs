using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public interface ILearningOutcomeService : IDocumentInAcademicYearService<LearningOutcomeDocument>
    {
        Task<List<string>> History(Guid id);
        Task<LearningOutcomeDocument> ImportFrom(Guid currentDocId, string fosCode, string academicYear);
        Task<LearningOutcomeDocument> Latest(string fosCode, string academicYear);
        Task<LearningOutcomeDocument> Save(LearningOutcomeDocument learningOutcome);
        Task<LearningOutcomeDocument> SaveAs(string fosCode, string academicYear, LearningOutcomeDocument learningOutcome);
    }
}