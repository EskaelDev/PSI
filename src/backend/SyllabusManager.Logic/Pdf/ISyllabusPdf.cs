using SyllabusManager.Data.Enums.LearningOutcomes;
using SyllabusManager.Data.Models.Syllabuses;
using System.Collections.Generic;

namespace SyllabusManager.Logic.Pdf
{
    public interface ISyllabusPdf
    {
        void Create(Syllabus syllabus, Dictionary<LearningOutcomeCategory, int> lods);
    }
}