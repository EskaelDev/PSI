using SyllabusManager.Data.Models.FieldOfStudies;
using System.Collections.Generic;

namespace SyllabusManager.Data.Models.LearningOutcomes
{
    public class LearningOutcomeDocument : DocumentInAcademicYearBase
    {
        public FieldOfStudy FieldOfStudy { get; set; }
        public List<LearningOutcome> LearningOutcomes { get; set; } = new List<LearningOutcome>();
    }
}
